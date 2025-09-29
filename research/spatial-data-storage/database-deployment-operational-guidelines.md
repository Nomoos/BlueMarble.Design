# Database Architecture Deployment and Operational Guidelines

## Executive Summary

This document provides comprehensive deployment and operational guidelines for implementing the Cassandra + Redis hybrid database architecture for BlueMarble's petabyte-scale 3D octree storage system. It covers infrastructure provisioning, configuration management, deployment automation, monitoring setup, and day-to-day operational procedures.

## Infrastructure Requirements

### Hardware Specifications

#### Cassandra Cluster Nodes

```yaml
production_cassandra_nodes:
  node_count: 6  # Initial cluster size
  instance_type: "r5.2xlarge"  # Memory-optimized
  specifications:
    cpu: "8 vCPUs"
    memory: "64 GB"
    storage: "4 TB NVMe SSD"
    network: "Up to 10 Gbps"
  operating_system: "Ubuntu 20.04 LTS"
  
staging_cassandra_nodes:
  node_count: 3
  instance_type: "r5.xlarge"
  specifications:
    cpu: "4 vCPUs" 
    memory: "32 GB"
    storage: "2 TB NVMe SSD"
    network: "Up to 10 Gbps"
    
development_cassandra_nodes:
  node_count: 1
  instance_type: "r5.large"
  specifications:
    cpu: "2 vCPUs"
    memory: "16 GB"
    storage: "500 GB NVMe SSD"
```

#### Redis Cluster Nodes

```yaml
production_redis_nodes:
  node_count: 6  # 3 masters + 3 replicas
  instance_type: "r6g.xlarge"  # ARM-based, memory-optimized
  specifications:
    cpu: "4 vCPUs"
    memory: "32 GB"
    storage: "SSD-backed memory"
    network: "Up to 10 Gbps"
    
staging_redis_nodes:
  node_count: 3  # 3 masters, no replicas
  instance_type: "r6g.large"
  specifications:
    cpu: "2 vCPUs"
    memory: "16 GB"
    storage: "SSD-backed memory"
```

### Network Architecture

```yaml
network_topology:
  vpc_configuration:
    cidr_block: "10.0.0.0/16"
    availability_zones: 3
    
  subnet_configuration:
    cassandra_subnet:
      cidr: "10.0.1.0/24"
      type: "private"
      availability_zones: ["us-west-2a", "us-west-2b", "us-west-2c"]
      
    redis_subnet:
      cidr: "10.0.2.0/24"
      type: "private"
      availability_zones: ["us-west-2a", "us-west-2b", "us-west-2c"]
      
    application_subnet:
      cidr: "10.0.3.0/24"
      type: "private"
      availability_zones: ["us-west-2a", "us-west-2b", "us-west-2c"]
      
    public_subnet:
      cidr: "10.0.100.0/24"
      type: "public"
      purpose: "Load balancers and NAT gateways"
      
  security_groups:
    cassandra_sg:
      inbound_rules:
        - port: 9042  # CQL
          source: "application_sg"
        - port: 7000  # Inter-node communication
          source: "cassandra_sg"
        - port: 7001  # TLS inter-node
          source: "cassandra_sg"
        - port: 7199  # JMX
          source: "monitoring_sg"
          
    redis_sg:
      inbound_rules:
        - port: 6379  # Redis
          source: "application_sg"
        - port: 16379 # Redis Cluster bus
          source: "redis_sg"
```

## Deployment Architecture

### Infrastructure as Code (Terraform)

```hcl
# main.tf
terraform {
  required_version = ">= 1.0"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
  
  backend "s3" {
    bucket = "bluemarble-terraform-state"
    key    = "database/terraform.tfstate"
    region = "us-west-2"
  }
}

# Cassandra cluster configuration
module "cassandra_cluster" {
  source = "./modules/cassandra"
  
  cluster_name     = var.cluster_name
  node_count       = var.cassandra_node_count
  instance_type    = var.cassandra_instance_type
  vpc_id           = aws_vpc.bluemarble.id
  subnet_ids       = aws_subnet.cassandra[*].id
  security_group_ids = [aws_security_group.cassandra.id]
  
  # Storage configuration
  root_volume_size = 100
  data_volume_size = var.cassandra_storage_size
  data_volume_type = "gp3"
  data_volume_iops = 16000
  
  # Cassandra-specific configuration
  cassandra_version = "4.0.6"
  heap_size        = "16G"
  replication_factor = 3
  
  tags = local.common_tags
}

# Redis cluster configuration  
module "redis_cluster" {
  source = "./modules/redis"
  
  cluster_name      = var.cluster_name
  node_count        = var.redis_node_count
  instance_type     = var.redis_instance_type
  vpc_id            = aws_vpc.bluemarble.id
  subnet_ids        = aws_subnet.redis[*].id
  security_group_ids = [aws_security_group.redis.id]
  
  # Redis-specific configuration
  redis_version     = "7.0.5"
  max_memory       = "24GB"
  maxmemory_policy = "allkeys-lru"
  cluster_enabled  = true
  
  tags = local.common_tags
}

# Monitoring infrastructure
module "monitoring" {
  source = "./modules/monitoring"
  
  cluster_name = var.cluster_name
  vpc_id       = aws_vpc.bluemarble.id
  subnet_ids   = aws_subnet.monitoring[*].id
  
  # Prometheus configuration
  prometheus_retention = "90d"
  prometheus_storage   = "500GB"
  
  # Grafana configuration
  grafana_admin_password = var.grafana_admin_password
  
  tags = local.common_tags
}
```

### Ansible Configuration Management

```yaml
# cassandra-playbook.yml
---
- name: Configure Cassandra Cluster
  hosts: cassandra_nodes
  become: yes
  vars:
    cassandra_version: "4.0.6"
    cassandra_heap_size: "16G"
    cassandra_cluster_name: "BlueMable"
    cassandra_data_dir: "/var/lib/cassandra"
    cassandra_log_dir: "/var/log/cassandra"
    
  tasks:
    - name: Install Java 11
      package:
        name: openjdk-11-jdk
        state: present
        
    - name: Add Cassandra repository
      apt_repository:
        repo: "deb https://debian.cassandra.apache.org 40x main"
        state: present
        
    - name: Install Cassandra
      package:
        name: "cassandra={{ cassandra_version }}"
        state: present
        
    - name: Configure Cassandra
      template:
        src: cassandra.yaml.j2
        dest: /etc/cassandra/cassandra.yaml
        backup: yes
      notify: restart cassandra
      
    - name: Configure JVM options
      template:
        src: jvm11-server.options.j2
        dest: /etc/cassandra/jvm11-server.options
        backup: yes
      notify: restart cassandra
      
    - name: Start and enable Cassandra service
      systemd:
        name: cassandra
        state: started
        enabled: yes
        
    - name: Wait for Cassandra to be ready
      wait_for:
        port: 9042
        delay: 30
        timeout: 300
        
  handlers:
    - name: restart cassandra
      systemd:
        name: cassandra
        state: restarted
```

```yaml
# redis-playbook.yml
---
- name: Configure Redis Cluster
  hosts: redis_nodes
  become: yes
  vars:
    redis_version: "7.0.5"
    redis_port: 6379
    redis_max_memory: "24gb"
    redis_data_dir: "/var/lib/redis"
    redis_log_dir: "/var/log/redis"
    
  tasks:
    - name: Install Redis
      package:
        name: "redis-server={{ redis_version }}"
        state: present
        
    - name: Configure Redis
      template:
        src: redis.conf.j2
        dest: /etc/redis/redis.conf
        backup: yes
      notify: restart redis
      
    - name: Create Redis data directory
      file:
        path: "{{ redis_data_dir }}"
        state: directory
        owner: redis
        group: redis
        mode: '0755'
        
    - name: Start and enable Redis service
      systemd:
        name: redis-server
        state: started
        enabled: yes
        
    - name: Wait for Redis to be ready
      wait_for:
        port: "{{ redis_port }}"
        delay: 10
        timeout: 60
        
  handlers:
    - name: restart redis
      systemd:
        name: redis-server
        state: restarted
```

### Database Schema Initialization

```sql
-- cassandra-schema.cql
CREATE KEYSPACE IF NOT EXISTS bluemarble
WITH replication = {
  'class': 'NetworkTopologyStrategy',
  'datacenter1': 3
}
AND durable_writes = true;

USE bluemarble;

-- Primary octree storage table
CREATE TABLE IF NOT EXISTS octree_nodes (
    morton_code BIGINT,
    level TINYINT,
    material_id INT,
    homogeneity FLOAT,
    children_mask TINYINT,
    compressed_data BLOB,
    last_modified TIMESTAMP,
    PRIMARY KEY ((morton_code, level))
) WITH compression = {'class': 'LZ4Compressor'}
  AND compaction = {
    'class': 'LeveledCompactionStrategy',
    'sstable_size_in_mb': 256
  }
  AND gc_grace_seconds = 86400;

-- Secondary index for material queries
CREATE INDEX IF NOT EXISTS idx_material_id 
ON octree_nodes (material_id);

-- Secondary index for temporal queries
CREATE INDEX IF NOT EXISTS idx_last_modified 
ON octree_nodes (last_modified);

-- Hot regions tracking table
CREATE TABLE IF NOT EXISTS hot_regions (
    region_id UUID,
    morton_code_start BIGINT,
    morton_code_end BIGINT,
    level TINYINT,
    access_count COUNTER,
    last_accessed TIMESTAMP,
    PRIMARY KEY (region_id)
) WITH compression = {'class': 'LZ4Compressor'};

-- User access patterns table
CREATE TABLE IF NOT EXISTS access_patterns (
    user_id UUID,
    access_date DATE,
    morton_code BIGINT,
    level TINYINT,
    access_count COUNTER,
    PRIMARY KEY ((user_id, access_date), morton_code, level)
) WITH compression = {'class': 'LZ4Compressor'}
  AND default_time_to_live = 2592000; -- 30 days
```

### Application Configuration

```csharp
// appsettings.Production.json
{
  "DatabaseSettings": {
    "Cassandra": {
      "ContactPoints": [
        "10.0.1.10",
        "10.0.1.11", 
        "10.0.1.12"
      ],
      "Port": 9042,
      "Keyspace": "bluemarble",
      "LocalDatacenter": "datacenter1",
      "DefaultConsistencyLevel": "LocalQuorum",
      "PoolingOptions": {
        "CoreConnectionsPerHost": 4,
        "MaxConnectionsPerHost": 8,
        "MaxRequestsPerConnection": 32768
      },
      "SocketOptions": {
        "ConnectTimeoutMillis": 5000,
        "ReadTimeoutMillis": 30000
      },
      "RetryPolicy": {
        "Type": "DefaultRetryPolicy"
      },
      "Compression": "LZ4"
    },
    "Redis": {
      "Configuration": "10.0.2.10:6379,10.0.2.11:6379,10.0.2.12:6379",
      "DatabaseId": 0,
      "ConnectTimeout": 5000,
      "SyncTimeout": 30000,
      "ConnectRetry": 3,
      "KeepAlive": 60,
      "DefaultTTL": "01:00:00",
      "KeyPrefix": "bluemarble:",
      "Compression": true
    }
  },
  "CachingSettings": {
    "HotRegionThreshold": 0.05,
    "CacheEvictionPolicy": "LRU",
    "MaxCacheSize": "16GB",
    "StatisticsEnabled": true
  },
  "PerformanceSettings": {
    "MaxConcurrentQueries": 1000,
    "QueryTimeoutSeconds": 30,
    "BatchSize": 100,
    "EnableQueryMetrics": true
  }
}
```

## Deployment Pipeline

### CI/CD Pipeline Configuration

```yaml
# .github/workflows/database-deployment.yml
name: Database Deployment Pipeline

on:
  push:
    branches: [main]
    paths: ['infrastructure/database/**']
  pull_request:
    branches: [main]
    paths: ['infrastructure/database/**']

env:
  AWS_REGION: us-west-2
  TERRAFORM_VERSION: 1.5.0
  ANSIBLE_VERSION: 6.0.0

jobs:
  terraform-plan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Terraform
        uses: hashicorp/setup-terraform@v2
        with:
          terraform_version: ${{ env.TERRAFORM_VERSION }}
          
      - name: Configure AWS credentials
        uses: aws-actions/configure-aws-credentials@v2
        with:
          aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
          aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
          aws-region: ${{ env.AWS_REGION }}
          
      - name: Terraform Init
        run: terraform init
        working-directory: infrastructure/database
        
      - name: Terraform Plan
        run: terraform plan -out=tfplan
        working-directory: infrastructure/database
        
      - name: Upload Terraform Plan
        uses: actions/upload-artifact@v3
        with:
          name: terraform-plan
          path: infrastructure/database/tfplan
          
  security-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Run Checkov
        uses: bridgecrewio/checkov-action@master
        with:
          directory: infrastructure/database
          framework: terraform
          
      - name: Run TFSec
        uses: aquasecurity/tfsec-action@v1.0.0
        with:
          working_directory: infrastructure/database
          
  deploy-staging:
    needs: [terraform-plan, security-scan]
    if: github.event_name == 'pull_request'
    runs-on: ubuntu-latest
    environment: staging
    steps:
      - uses: actions/checkout@v3
      
      - name: Download Terraform Plan
        uses: actions/download-artifact@v3
        with:
          name: terraform-plan
          path: infrastructure/database
          
      - name: Setup Terraform
        uses: hashicorp/setup-terraform@v2
        with:
          terraform_version: ${{ env.TERRAFORM_VERSION }}
          
      - name: Configure AWS credentials
        uses: aws-actions/configure-aws-credentials@v2
        with:
          aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
          aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
          aws-region: ${{ env.AWS_REGION }}
          
      - name: Terraform Apply
        run: terraform apply -auto-approve tfplan
        working-directory: infrastructure/database
        
      - name: Run Integration Tests
        run: |
          cd tests/integration
          python -m pytest database_tests.py -v
          
  deploy-production:
    needs: [terraform-plan, security-scan]
    if: github.ref == 'refs/heads/main'
    runs-on: ubuntu-latest
    environment: production
    steps:
      - uses: actions/checkout@v3
      
      - name: Manual Approval Required
        uses: trstringer/manual-approval@v1
        with:
          secret: ${{ github.TOKEN }}
          approvers: database-admins
          minimum-approvals: 2
          
      - name: Deploy to Production
        run: |
          # Production deployment with additional safety checks
          terraform init
          terraform plan -detailed-exitcode
          terraform apply -auto-approve
        working-directory: infrastructure/database
```

### Configuration Management Automation

```bash
#!/bin/bash
# deploy-database-cluster.sh

set -euo pipefail

ENVIRONMENT=${1:-staging}
CLUSTER_NAME="bluemarble-${ENVIRONMENT}"
CONFIG_DIR="./config/${ENVIRONMENT}"

echo "Starting database cluster deployment for environment: ${ENVIRONMENT}"

# Step 1: Deploy infrastructure with Terraform
echo "Deploying infrastructure..."
cd infrastructure/database
terraform workspace select ${ENVIRONMENT} || terraform workspace new ${ENVIRONMENT}
terraform init
terraform plan -var-file="${CONFIG_DIR}/terraform.tfvars" -out=tfplan
terraform apply tfplan

# Get infrastructure outputs
CASSANDRA_IPS=$(terraform output -json cassandra_private_ips | jq -r '.[]')
REDIS_IPS=$(terraform output -json redis_private_ips | jq -r '.[]')

# Step 2: Generate Ansible inventory
echo "Generating Ansible inventory..."
cat > inventory.ini << EOF
[cassandra_nodes]
$(echo "$CASSANDRA_IPS" | while read ip; do echo "$ip ansible_user=ubuntu"; done)

[redis_nodes] 
$(echo "$REDIS_IPS" | while read ip; do echo "$ip ansible_user=ubuntu"; done)

[all:vars]
ansible_ssh_private_key_file=~/.ssh/bluemarble-${ENVIRONMENT}.pem
EOF

# Step 3: Configure nodes with Ansible
echo "Configuring Cassandra nodes..."
ansible-playbook -i inventory.ini cassandra-playbook.yml \
  --extra-vars "cluster_name=${CLUSTER_NAME}"

echo "Configuring Redis nodes..."
ansible-playbook -i inventory.ini redis-playbook.yml \
  --extra-vars "cluster_name=${CLUSTER_NAME}"

# Step 4: Initialize database schemas
echo "Initializing database schemas..."
CASSANDRA_HOST=$(echo "$CASSANDRA_IPS" | head -n1)
cqlsh ${CASSANDRA_HOST} -f schemas/cassandra-schema.cql

# Step 5: Configure Redis cluster
echo "Configuring Redis cluster..."
redis-cli --cluster create ${REDIS_IPS// /:6379 }:6379 \
  --cluster-replicas 1 --cluster-yes

# Step 6: Run health checks
echo "Running health checks..."
python scripts/health-check.py --environment ${ENVIRONMENT}

echo "Database cluster deployment completed successfully!"
```

## Monitoring and Observability

### Prometheus Configuration

```yaml
# prometheus.yml
global:
  scrape_interval: 15s
  evaluation_interval: 15s

rule_files:
  - "cassandra-alerts.yml"
  - "redis-alerts.yml"
  - "application-alerts.yml"

scrape_configs:
  # Cassandra JMX metrics
  - job_name: 'cassandra'
    static_configs:
      - targets: 
        - '10.0.1.10:7199'
        - '10.0.1.11:7199'
        - '10.0.1.12:7199'
    metrics_path: /metrics
    scrape_interval: 30s
    
  # Redis metrics
  - job_name: 'redis'
    static_configs:
      - targets:
        - '10.0.2.10:9121'
        - '10.0.2.11:9121'
        - '10.0.2.12:9121'
    metrics_path: /metrics
    scrape_interval: 15s
    
  # Application metrics
  - job_name: 'bluemarble-api'
    static_configs:
      - targets:
        - '10.0.3.10:8080'
        - '10.0.3.11:8080'
    metrics_path: /metrics
    scrape_interval: 10s

alerting:
  alertmanagers:
    - static_configs:
        - targets:
          - 'alertmanager:9093'
```

### Alert Rules

```yaml
# cassandra-alerts.yml
groups:
  - name: cassandra.rules
    rules:
      - alert: CassandraNodeDown
        expr: up{job="cassandra"} == 0
        for: 1m
        labels:
          severity: critical
        annotations:
          summary: "Cassandra node {{ $labels.instance }} is down"
          description: "Cassandra node has been down for more than 1 minute"
          
      - alert: CassandraHighReadLatency
        expr: cassandra_read_latency_p95 > 0.1
        for: 5m
        labels:
          severity: warning
        annotations:
          summary: "High read latency on Cassandra cluster"
          description: "95th percentile read latency is {{ $value }}s"
          
      - alert: CassandraHighWriteLatency
        expr: cassandra_write_latency_p95 > 0.2
        for: 5m
        labels:
          severity: warning
        annotations:
          summary: "High write latency on Cassandra cluster"
          description: "95th percentile write latency is {{ $value }}s"
          
      - alert: CassandraHighCompactionPending
        expr: cassandra_compaction_pending_tasks > 100
        for: 10m
        labels:
          severity: warning
        annotations:
          summary: "High number of pending compaction tasks"
          description: "{{ $value }} compaction tasks are pending"
          
      - alert: CassandraLowDiskSpace
        expr: cassandra_disk_usage_percentage > 80
        for: 5m
        labels:
          severity: warning
        annotations:
          summary: "Low disk space on Cassandra node"
          description: "Disk usage is {{ $value }}% on {{ $labels.instance }}"

  - name: redis.rules
    rules:
      - alert: RedisNodeDown
        expr: up{job="redis"} == 0
        for: 1m
        labels:
          severity: critical
        annotations:
          summary: "Redis node {{ $labels.instance }} is down"
          
      - alert: RedisHighMemoryUsage
        expr: redis_memory_usage_percentage > 90
        for: 5m
        labels:
          severity: warning
        annotations:
          summary: "High memory usage on Redis cluster"
          description: "Memory usage is {{ $value }}%"
          
      - alert: RedisLowCacheHitRate
        expr: redis_cache_hit_rate < 0.8
        for: 10m
        labels:
          severity: warning
        annotations:
          summary: "Low cache hit rate"
          description: "Cache hit rate is {{ $value }}"
          
      - alert: RedisHighEvictionRate
        expr: rate(redis_evicted_keys_total[5m]) > 100
        for: 5m
        labels:
          severity: warning
        annotations:
          summary: "High key eviction rate"
          description: "Evicting {{ $value }} keys per second"
```

### Grafana Dashboards

```json
{
  "dashboard": {
    "title": "BlueMarble Database Overview",
    "panels": [
      {
        "title": "Cassandra Cluster Health",
        "type": "stat",
        "targets": [
          {
            "expr": "up{job=\"cassandra\"}",
            "legendFormat": "Node {{instance}}"
          }
        ]
      },
      {
        "title": "Query Latency",
        "type": "graph",
        "targets": [
          {
            "expr": "cassandra_read_latency_p95",
            "legendFormat": "Read P95"
          },
          {
            "expr": "cassandra_write_latency_p95", 
            "legendFormat": "Write P95"
          }
        ]
      },
      {
        "title": "Redis Cache Performance",
        "type": "graph",
        "targets": [
          {
            "expr": "redis_cache_hit_rate",
            "legendFormat": "Hit Rate"
          },
          {
            "expr": "redis_memory_usage_percentage",
            "legendFormat": "Memory Usage %"
          }
        ]
      }
    ]
  }
}
```

## Operational Procedures

### Daily Operations Checklist

```yaml
daily_operations:
  monitoring_checks:
    - check: "Verify all nodes are healthy in monitoring dashboard"
      frequency: "Every 4 hours"
      owner: "Operations team"
      
    - check: "Review query latency metrics and trends"
      frequency: "Morning review"
      owner: "Database team"
      
    - check: "Check disk space utilization across all nodes"
      frequency: "Daily"
      owner: "Operations team"
      
    - check: "Review error logs for any anomalies"
      frequency: "Daily"
      owner: "Database team"
      
  maintenance_tasks:
    - task: "Backup verification and integrity check"
      frequency: "Daily"
      automation_level: "Fully automated"
      
    - task: "Compaction monitoring and optimization"
      frequency: "Daily" 
      automation_level: "Automated with manual review"
      
    - task: "Cache performance analysis"
      frequency: "Daily"
      automation_level: "Automated reporting"
```

### Weekly Operations Checklist

```yaml
weekly_operations:
  performance_review:
    - task: "Analyze query performance trends"
      day: "Monday"
      duration: "30 minutes"
      
    - task: "Review capacity planning metrics"
      day: "Wednesday"
      duration: "45 minutes"
      
    - task: "Evaluate cache hit rate patterns"
      day: "Friday"
      duration: "20 minutes"
      
  maintenance_tasks:
    - task: "Run full consistency check on sample data"
      day: "Sunday"
      duration: "2 hours"
      
    - task: "Update security patches (staging first)"
      day: "Wednesday"
      duration: "4 hours"
      
    - task: "Backup and recovery test"
      day: "Saturday"
      duration: "3 hours"
```

### Backup and Recovery Procedures

```bash
#!/bin/bash
# backup-cassandra.sh

BACKUP_DATE=$(date +%Y%m%d_%H%M%S)
BACKUP_DIR="/backups/cassandra/${BACKUP_DATE}"
KEYSPACE="bluemarble"

echo "Starting Cassandra backup: ${BACKUP_DATE}"

# Create backup directory
mkdir -p ${BACKUP_DIR}

# Get list of all nodes
NODES=$(nodetool status | grep '^UN' | awk '{print $2}')

# Create snapshots on all nodes
for node in ${NODES}; do
    echo "Creating snapshot on node: ${node}"
    nodetool -h ${node} snapshot ${KEYSPACE} -t ${BACKUP_DATE}
done

# Copy snapshot data
for node in ${NODES}; do
    echo "Copying data from node: ${node}"
    node_dir="${BACKUP_DIR}/${node}"
    mkdir -p ${node_dir}
    
    # Use rsync to copy snapshot data
    rsync -av ${node}:/var/lib/cassandra/data/${KEYSPACE}/*/snapshots/${BACKUP_DATE}/ \
          ${node_dir}/
done

# Create backup metadata
cat > ${BACKUP_DIR}/backup_info.json << EOF
{
  "backup_date": "${BACKUP_DATE}",
  "keyspace": "${KEYSPACE}",
  "nodes": [$(echo ${NODES} | sed 's/ /", "/g' | sed 's/^/"/' | sed 's/$/"/')],
  "backup_type": "snapshot",
  "compression": "none",
  "encryption": "none"
}
EOF

# Compress backup
echo "Compressing backup..."
tar -czf ${BACKUP_DIR}.tar.gz -C /backups/cassandra ${BACKUP_DATE}

# Upload to S3
echo "Uploading to S3..."
aws s3 cp ${BACKUP_DIR}.tar.gz s3://bluemarble-backups/cassandra/

# Cleanup local files older than 7 days
find /backups/cassandra -name "*.tar.gz" -mtime +7 -delete

echo "Backup completed: ${BACKUP_DIR}.tar.gz"
```

### Performance Tuning Procedures

```csharp
public class PerformanceTuningService
{
    public class TuningRecommendation
    {
        public string Component { get; set; }
        public string Parameter { get; set; }
        public string CurrentValue { get; set; }
        public string RecommendedValue { get; set; }
        public string Rationale { get; set; }
        public string ImpactLevel { get; set; }
    }
    
    public async Task<List<TuningRecommendation>> AnalyzePerformance()
    {
        var recommendations = new List<TuningRecommendation>();
        
        // Analyze Cassandra performance
        var cassandraMetrics = await GetCassandraMetrics();
        if (cassandraMetrics.ReadLatencyP95 > 0.01) // 10ms
        {
            recommendations.Add(new TuningRecommendation
            {
                Component = "Cassandra",
                Parameter = "read_request_timeout",
                CurrentValue = "5000ms",
                RecommendedValue = "3000ms",
                Rationale = "Reduce timeout to fail fast on slow queries",
                ImpactLevel = "Medium"
            });
        }
        
        if (cassandraMetrics.CompactionPendingTasks > 50)
        {
            recommendations.Add(new TuningRecommendation
            {
                Component = "Cassandra",
                Parameter = "concurrent_compactors",
                CurrentValue = "4",
                RecommendedValue = "8",
                Rationale = "Increase compaction throughput",
                ImpactLevel = "High"
            });
        }
        
        // Analyze Redis performance
        var redisMetrics = await GetRedisMetrics();
        if (redisMetrics.CacheHitRate < 0.85)
        {
            recommendations.Add(new TuningRecommendation
            {
                Component = "Redis",
                Parameter = "maxmemory",
                CurrentValue = "16GB",
                RecommendedValue = "24GB",
                Rationale = "Increase cache size to improve hit rate",
                ImpactLevel = "High"
            });
        }
        
        return recommendations;
    }
}
```

## Security Operational Procedures

### Access Control Management

```yaml
security_procedures:
  access_control:
    cassandra_users:
      - username: "bluemarble_app"
        roles: ["read_write_bluemarble"]
        password_rotation: "90 days"
        
      - username: "bluemarble_monitor"
        roles: ["read_only_metrics"]
        password_rotation: "90 days"
        
      - username: "backup_user"
        roles: ["backup_operator"]
        password_rotation: "60 days"
        
    redis_acls:
      - username: "app_user"
        commands: ["+@read", "+@write", "-flushdb", "-flushall"]
        keys: ["bluemarble:*"]
        
      - username: "monitor_user"
        commands: ["+info", "+ping", "+client"]
        keys: ["*"]
        
  certificate_management:
    ssl_certificates:
      - service: "Cassandra inter-node"
        certificate_path: "/etc/cassandra/certs/"
        renewal_frequency: "annually"
        
      - service: "Redis TLS"
        certificate_path: "/etc/redis/certs/"
        renewal_frequency: "annually"
        
  audit_procedures:
    log_retention: "7 years"
    audit_frequency: "monthly"
    compliance_reports: "quarterly"
```

### Security Monitoring

```bash
#!/bin/bash
# security-audit.sh

AUDIT_DATE=$(date +%Y%m%d)
AUDIT_REPORT="/var/log/security/audit_${AUDIT_DATE}.log"

echo "Starting security audit: ${AUDIT_DATE}" > ${AUDIT_REPORT}

# Check for unusual access patterns
echo "=== Unusual Access Patterns ===" >> ${AUDIT_REPORT}
grep "authentication failure" /var/log/auth.log | tail -20 >> ${AUDIT_REPORT}

# Check Cassandra audit logs
echo "=== Cassandra Audit Log ===" >> ${AUDIT_REPORT}
grep "DDL\|DML" /var/log/cassandra/audit.log | tail -50 >> ${AUDIT_REPORT}

# Check Redis command audit
echo "=== Redis Command Audit ===" >> ${AUDIT_REPORT}
grep "FLUSHDB\|FLUSHALL\|CONFIG" /var/log/redis/redis.log >> ${AUDIT_REPORT}

# Check for privilege escalation attempts
echo "=== Privilege Escalation ===" >> ${AUDIT_REPORT}
grep "sudo" /var/log/auth.log | grep "$(date +%Y-%m-%d)" >> ${AUDIT_REPORT}

# Send report to security team
mail -s "Database Security Audit Report - ${AUDIT_DATE}" \
     security-team@bluemarble.com < ${AUDIT_REPORT}
```

## Disaster Recovery Procedures

### Automated Failover Configuration

```yaml
# consul-template for automated failover
consul_template_config:
  templates:
    - source: "/etc/consul-templates/cassandra-endpoints.tpl"
      destination: "/etc/bluemarble/cassandra-endpoints.conf"
      command: "systemctl reload bluemarble-api"
      
  watch_config:
    cassandra_health_check:
      service: "cassandra"
      interval: "30s"
      timeout: "10s"
      
    redis_health_check:
      service: "redis"
      interval: "15s"
      timeout: "5s"
```

### Manual Disaster Recovery Playbook

```markdown
# Disaster Recovery Playbook

## Scenario 1: Complete Datacenter Failure

### Immediate Actions (0-15 minutes)
1. **Assess Impact**: Confirm datacenter is completely unavailable
2. **Activate DR Site**: Initiate failover to backup datacenter
3. **Update DNS**: Point traffic to DR site
4. **Notify Stakeholders**: Send incident notification

### Recovery Actions (15 minutes - 4 hours)
1. **Verify DR Systems**: Confirm all services operational in DR site
2. **Data Validation**: Run consistency checks on DR data
3. **Performance Testing**: Verify system performance meets SLAs
4. **Monitor Closely**: Watch for any issues with DR environment

### Post-Recovery Actions (After primary recovery)
1. **Sync Data**: Ensure all data changes are synchronized
2. **Failback Planning**: Plan return to primary datacenter
3. **Post-Mortem**: Conduct incident review and improvement planning

## Scenario 2: Database Corruption

### Immediate Actions (0-30 minutes)
1. **Stop Writes**: Immediately put application in read-only mode
2. **Assess Extent**: Determine scope of corruption
3. **Isolate Affected Nodes**: Remove corrupted nodes from cluster
4. **Initiate Recovery**: Begin restoration from latest valid backup

### Recovery Actions (30 minutes - 6 hours)
1. **Data Restoration**: Restore from backup to new nodes
2. **Consistency Validation**: Verify data integrity post-restoration
3. **Gradual Traffic Return**: Slowly restore write capabilities
4. **Full System Test**: Complete end-to-end testing
```

## Cost Optimization

### Resource Optimization Strategies

```yaml
cost_optimization:
  compute_optimization:
    - strategy: "Auto-scaling based on demand"
      implementation: "CloudWatch metrics + Auto Scaling Groups"
      savings_potential: "25-40%"
      
    - strategy: "Reserved instances for stable workloads"
      implementation: "1-year reserved instances for production"
      savings_potential: "40-60%"
      
    - strategy: "Spot instances for batch processing"
      implementation: "Use spot instances for data migration tasks"
      savings_potential: "60-90%"
      
  storage_optimization:
    - strategy: "Data lifecycle management"
      implementation: "Archive old data to cheaper storage tiers"
      savings_potential: "50-70%"
      
    - strategy: "Compression optimization"
      implementation: "LZ4 compression + data modeling optimization"
      savings_potential: "60-80%"
      
  monitoring_optimization:
    - strategy: "Right-size monitoring retention"
      implementation: "Reduce metric retention for non-critical metrics"
      savings_potential: "20-30%"
```

### Cost Monitoring and Alerting

```csharp
public class CostMonitoringService
{
    public class CostAlert
    {
        public string AlertName { get; set; }
        public decimal ThresholdAmount { get; set; }
        public string Period { get; set; }
        public List<string> NotificationTargets { get; set; }
    }
    
    public static readonly CostAlert[] CostAlerts = {
        new() {
            AlertName = "Daily spend spike",
            ThresholdAmount = 1000m,
            Period = "Daily",
            NotificationTargets = new() { "ops-team@bluemarble.com", "finance@bluemarble.com" }
        },
        new() {
            AlertName = "Monthly budget exceeded",
            ThresholdAmount = 25000m,
            Period = "Monthly",
            NotificationTargets = new() { "executives@bluemarble.com", "finance@bluemarble.com" }
        }
    };
    
    public async Task<CostReport> GenerateMonthlyCostReport()
    {
        return new CostReport
        {
            TotalCost = await GetTotalMonthlyCost(),
            CostByService = await GetCostBreakdownByService(),
            CostTrends = await GetCostTrends(),
            OptimizationRecommendations = await GetCostOptimizationRecommendations(),
            BudgetVariance = await CalculateBudgetVariance()
        };
    }
}
```

## Conclusion

This comprehensive deployment and operational guide provides all necessary procedures for successfully implementing and maintaining the Cassandra + Redis hybrid database architecture for BlueMarble's petabyte-scale 3D octree storage system.

**Key Success Factors**:
1. **Automated Deployment**: Infrastructure as Code ensures consistent and repeatable deployments
2. **Comprehensive Monitoring**: Proactive monitoring prevents issues before they impact users
3. **Robust Backup Strategy**: Multiple backup tiers ensure data protection
4. **Clear Operational Procedures**: Well-defined processes enable efficient operations
5. **Security First**: Security measures integrated throughout the operational lifecycle

**Next Steps**:
1. Review and customize configurations for specific environment requirements
2. Train operations team on all procedures and tools
3. Execute deployment in staging environment for validation
4. Conduct disaster recovery testing
5. Plan production deployment with appropriate change management procedures

This operational framework provides the foundation for reliable, secure, and cost-effective operation of BlueMarble's database architecture at petabyte scale.
