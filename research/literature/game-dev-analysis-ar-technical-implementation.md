# AR Technical Implementation Guide: Unity, ARKit, and ARCore

---
title: AR Technical Implementation - Unity AR Foundation, ARKit, and ARCore for BlueMarble
date: 2025-01-15
tags: [unity, arkit, arcore, ar-foundation, technical-implementation, mobile-ar, ios, android]
status: complete
priority: medium
parent-research: game-dev-analysis-ar-concepts.md
---

**Sources:**  
- Unity AR & VR by Tutorials (raywenderlich.com)
- ARKit Documentation (Apple)
- ARCore Documentation (Google)

**Category:** GameDev-Tech  
**Priority:** Medium  
**Status:** ✅ Complete  
**Lines:** 850  
**Related Sources:** AR Concepts Analysis, Minecraft Earth Case Study

---

## Executive Summary

This technical implementation guide synthesizes practical AR development knowledge from Unity AR Foundation, Apple's 
ARKit, and Google's ARCore to provide a comprehensive reference for implementing AR features in BlueMarble's mobile 
companion app. The guide focuses on cross-platform AR development using Unity AR Foundation as the abstraction layer, 
with platform-specific optimizations for iOS (ARKit) and Android (ARCore).

**Key Takeaways for BlueMarble:**
- Unity AR Foundation provides cross-platform abstraction over ARKit and ARCore
- Plane detection and tracking are core capabilities for AR placement
- Light estimation enables realistic object rendering
- Image tracking supports marker-based AR experiences
- Environmental understanding (meshing) enables advanced interactions
- Performance optimization critical for mobile AR (60 FPS target)
- Platform-specific features require conditional compilation

**Implementation Priority:** Medium - Technical foundation for Phase 2/3 AR features

---

## Part I: Unity AR Foundation Overview

### 1. AR Foundation Architecture

**Unity AR Foundation Structure:**
```
Unity AR Foundation (Cross-platform API)
    ├── ARKit XR Plugin (iOS)
    ├── ARCore XR Plugin (Android)
    └── Universal Render Pipeline (URP) integration

Core Components:
- AR Session: Manages AR lifecycle
- AR Session Origin: Coordinate system anchor
- AR Camera: Modified camera for AR rendering
- AR Raycast Manager: Raycasting against AR surfaces
- AR Plane Manager: Detects and tracks planar surfaces
- AR Anchor Manager: Persistent world anchors
```

**BlueMarble Implementation Setup:**
```csharp
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BlueMarbleARManager : MonoBehaviour
{
    // Core AR Foundation components
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARSessionOrigin arSessionOrigin;
    [SerializeField] private ARCameraManager arCameraManager;
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private ARAnchorManager arAnchorManager;
    
    // AR state tracking
    private bool isARSupported = false;
    private bool isARActive = false;
    
    private void Awake()
    {
        // Check AR support
        CheckARSupport();
    }
    
    private async void CheckARSupport()
    {
        // Check if device supports AR
        if (ARSession.state == ARSessionState.None || 
            ARSession.state == ARSessionState.CheckingAvailability)
        {
            await ARSession.CheckAvailability();
        }
        
        isARSupported = ARSession.state != ARSessionState.Unsupported;
        
        if (!isARSupported)
        {
            ShowMessage("AR not supported on this device");
            // Fallback to non-AR mode
            EnableNonARMode();
        }
        else
        {
            // Initialize AR session
            InitializeAR();
        }
    }
    
    private void InitializeAR()
    {
        // Enable AR session
        arSession.enabled = true;
        
        // Configure plane detection
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
        
        // Enable light estimation
        if (arCameraManager.requestedLightEstimation == LightEstimation.None)
        {
            arCameraManager.requestedLightEstimation = LightEstimation.AmbientIntensity;
        }
        
        // Subscribe to AR events
        SubscribeToAREvents();
        
        isARActive = true;
        
        ShowMessage("AR initialized successfully");
    }
    
    private void SubscribeToAREvents()
    {
        // Plane detection events
        arPlaneManager.planesChanged += OnPlanesChanged;
        
        // AR session state changes
        ARSession.stateChanged += OnARSessionStateChanged;
    }
    
    private void OnARSessionStateChanged(ARSessionStateChangedEventArgs args)
    {
        switch (args.state)
        {
            case ARSessionState.SessionInitializing:
                ShowMessage("Initializing AR...");
                break;
                
            case ARSessionState.SessionTracking:
                ShowMessage("AR tracking active");
                break;
                
            case ARSessionState.SessionInterrupted:
                ShowMessage("AR tracking interrupted");
                PauseARFeatures();
                break;
        }
    }
}
```

---

### 2. Plane Detection and Surface Tracking

**Detecting Surfaces for Object Placement:**
```csharp
public class ARSurfaceDetection : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private Material planeMaterial; // Visual feedback
    
    private List<ARPlane> detectedPlanes = new List<ARPlane>();
    
    private void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }
    
    private void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }
    
    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        // Handle newly added planes
        foreach (ARPlane plane in args.added)
        {
            OnPlaneAdded(plane);
        }
        
        // Handle updated planes
        foreach (ARPlane plane in args.updated)
        {
            OnPlaneUpdated(plane);
        }
        
        // Handle removed planes
        foreach (ARPlane plane in args.removed)
        {
            OnPlaneRemoved(plane);
        }
    }
    
    private void OnPlaneAdded(ARPlane plane)
    {
        detectedPlanes.Add(plane);
        
        // Apply visual material
        MeshRenderer renderer = plane.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = planeMaterial;
        }
        
        // Check if this is a suitable surface for placement
        if (IsSuitableForPlacement(plane))
        {
            // Notify player that surface is ready
            ShowPlacementIndicator(plane);
        }
    }
    
    private bool IsSuitableForPlacement(ARPlane plane)
    {
        // Minimum size requirements for buildplate placement
        const float MIN_AREA = 0.25f; // 0.5m x 0.5m
        
        if (plane.alignment != PlaneAlignment.HorizontalUp)
            return false; // Only horizontal surfaces
        
        if (plane.size.x * plane.size.y < MIN_AREA)
            return false; // Too small
        
        return true;
    }
    
    private void OnPlaneUpdated(ARPlane plane)
    {
        // Plane boundaries have been refined
        // Update any objects anchored to this plane
        UpdatePlacedObjects(plane);
    }
    
    private void OnPlaneRemoved(ARPlane plane)
    {
        detectedPlanes.Remove(plane);
        
        // Handle objects that were on this plane
        HandlePlaneRemoval(plane);
    }
}
```

**BlueMarble Application - Buildplate Placement:**
```csharp
public class BuildplatePlacement : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject buildplatePrefab;
    
    private GameObject placedBuildplate;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    
    private void Update()
    {
        // Check for touch input
        if (Input.touchCount == 0)
            return;
        
        Touch touch = Input.GetTouch(0);
        
        // On tap, try to place buildplate
        if (touch.phase == TouchPhase.Began)
        {
            TryPlaceBuildplate(touch.position);
        }
    }
    
    private void TryPlaceBuildplate(Vector2 screenPosition)
    {
        // Raycast against AR planes
        if (raycastManager.Raycast(screenPosition, raycastHits, 
            TrackableType.PlaneWithinPolygon))
        {
            // Get first hit
            ARRaycastHit hit = raycastHits[0];
            
            // Check if already placed
            if (placedBuildplate != null)
            {
                // Move existing buildplate
                MoveBuildplate(hit.pose);
            }
            else
            {
                // Place new buildplate
                PlaceBuildplate(hit.pose);
            }
        }
    }
    
    private void PlaceBuildplate(Pose pose)
    {
        // Instantiate buildplate at hit position
        placedBuildplate = Instantiate(buildplatePrefab, 
                                       pose.position, 
                                       pose.rotation);
        
        // Create AR anchor for persistence
        ARAnchor anchor = placedBuildplate.AddComponent<ARAnchor>();
        
        // Notify game logic
        OnBuildplatePlaced(placedBuildplate);
        
        ShowMessage("Buildplate placed! Start building.");
    }
    
    private void MoveBuildplate(Pose newPose)
    {
        // Update buildplate position
        placedBuildplate.transform.SetPositionAndRotation(
            newPose.position, 
            newPose.rotation);
        
        // Update anchor
        ARAnchor anchor = placedBuildplate.GetComponent<ARAnchor>();
        if (anchor != null)
        {
            Destroy(anchor);
        }
        placedBuildplate.AddComponent<ARAnchor>();
    }
}
```

---

### 3. Light Estimation for Realistic Rendering

**Matching AR Objects to Real-World Lighting:**
```csharp
public class ARLightEstimation : MonoBehaviour
{
    [SerializeField] private ARCameraManager cameraManager;
    [SerializeField] private Light directionalLight;
    
    private void OnEnable()
    {
        if (cameraManager != null)
        {
            cameraManager.frameReceived += OnCameraFrameReceived;
        }
    }
    
    private void OnDisable()
    {
        if (cameraManager != null)
        {
            cameraManager.frameReceived -= OnCameraFrameReceived;
        }
    }
    
    private void OnCameraFrameReceived(ARCameraFrameEventArgs args)
    {
        // Get light estimation data
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            // Update directional light intensity
            float brightness = args.lightEstimation.averageBrightness.Value;
            directionalLight.intensity = brightness;
        }
        
        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            // Update light color temperature
            float temperature = args.lightEstimation.averageColorTemperature.Value;
            directionalLight.color = Mathf.CorrelatedColorTemperatureToRGB(temperature);
        }
        
        if (args.lightEstimation.colorCorrection.HasValue)
        {
            // Apply color correction to materials
            Color colorCorrection = args.lightEstimation.colorCorrection.Value;
            ApplyColorCorrection(colorCorrection);
        }
    }
    
    private void ApplyColorCorrection(Color correction)
    {
        // Update shader global parameters
        Shader.SetGlobalColor("_ARColorCorrection", correction);
    }
}
```

---

## Part II: Platform-Specific Features

### 4. ARKit (iOS) Advanced Features

**People Occlusion (ARKit 3+):**
```csharp
#if UNITY_IOS
using Unity.XR.ARKit;

public class ARKitOcclusion : MonoBehaviour
{
    [SerializeField] private AROcclusionManager occlusionManager;
    
    private void Start()
    {
        // Enable people occlusion
        if (ARKitSession.supportsOcclusionPreference)
        {
            occlusionManager.requestedOcclusionPreferenceMode = 
                OcclusionPreferenceMode.PreferEnvironmentOcclusion;
        }
    }
}
#endif
```

**Face Tracking (ARKit):**
```csharp
#if UNITY_IOS
public class ARKitFaceTracking : MonoBehaviour
{
    [SerializeField] private ARFaceManager faceManager;
    
    private void OnEnable()
    {
        faceManager.facesChanged += OnFacesChanged;
    }
    
    private void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        // Handle detected faces
        foreach (ARFace face in args.added)
        {
            // Could be used for avatar customization
            ProcessFaceData(face);
        }
    }
    
    private void ProcessFaceData(ARFace face)
    {
        // Access blend shapes for facial expressions
        // Could be used for character creation in main game
    }
}
#endif
```

---

### 5. ARCore (Android) Advanced Features

**Cloud Anchors for Shared AR:**
```csharp
#if UNITY_ANDROID
using Google.XR.ARCoreExtensions;

public class ARCoreCloudAnchors : MonoBehaviour
{
    [SerializeField] private ARAnchorManager anchorManager;
    
    public async void HostCloudAnchor(ARAnchor anchor)
    {
        // Host anchor to Google Cloud
        ARCloudAnchor cloudAnchor = anchorManager.HostCloudAnchor(anchor);
        
        if (cloudAnchor == null)
        {
            ShowError("Failed to host cloud anchor");
            return;
        }
        
        // Wait for hosting to complete
        while (cloudAnchor.cloudAnchorState == CloudAnchorState.TaskInProgress)
        {
            await Task.Yield();
        }
        
        if (cloudAnchor.cloudAnchorState == CloudAnchorState.Success)
        {
            // Cloud anchor ID can be shared with other players
            string cloudAnchorId = cloudAnchor.cloudAnchorId;
            ShareAnchorID(cloudAnchorId);
        }
    }
    
    public async void ResolveCloudAnchor(string cloudAnchorId)
    {
        // Resolve anchor from cloud
        ARCloudAnchor cloudAnchor = anchorManager.ResolveCloudAnchorId(cloudAnchorId);
        
        if (cloudAnchor == null)
        {
            ShowError("Failed to resolve cloud anchor");
            return;
        }
        
        // Wait for resolution
        while (cloudAnchor.cloudAnchorState == CloudAnchorState.TaskInProgress)
        {
            await Task.Yield();
        }
        
        if (cloudAnchor.cloudAnchorState == CloudAnchorState.Success)
        {
            // Anchor resolved - can place shared content
            OnCloudAnchorResolved(cloudAnchor);
        }
    }
}
#endif
```

---

## Part III: Performance Optimization

### 6. Mobile AR Performance Best Practices

**Frame Rate Optimization:**
```csharp
public class ARPerformanceManager : MonoBehaviour
{
    private const float TARGET_FRAME_TIME = 0.0167f; // 60 FPS
    
    [SerializeField] private int targetFrameRate = 60;
    [SerializeField] private bool enableDynamicResolution = true;
    
    private void Start()
    {
        // Set target frame rate
        Application.targetFrameRate = targetFrameRate;
        
        // Enable dynamic resolution for performance
        if (enableDynamicResolution)
        {
            QualitySettings.renderPipeline.dynamicResolutionEnabled = true;
        }
        
        // Optimize Unity settings for AR
        OptimizeQualitySettings();
    }
    
    private void OptimizeQualitySettings()
    {
        // Reduce shadow quality for mobile
        QualitySettings.shadows = ShadowQuality.Disable;
        
        // Limit shadow distance
        QualitySettings.shadowDistance = 20f;
        
        // Optimize texture quality
        QualitySettings.masterTextureLimit = 1; // Half resolution
        
        // Disable anti-aliasing (expensive on mobile)
        QualitySettings.antiAliasing = 0;
        
        // Limit pixel lights
        QualitySettings.pixelLightCount = 1;
    }
    
    private void Update()
    {
        // Monitor performance
        float frameTime = Time.deltaTime;
        
        if (frameTime > TARGET_FRAME_TIME * 1.5f)
        {
            // Performance issue - reduce quality
            ReduceQuality();
        }
    }
    
    private void ReduceQuality()
    {
        // Dynamic quality reduction
        // Reduce particle effects
        // Simplify 3D models (LOD)
        // Reduce draw distance
    }
}
```

**Battery Optimization:**
```csharp
public class ARBatteryManager : MonoBehaviour
{
    private const float LOW_BATTERY_THRESHOLD = 0.20f; // 20%
    
    [SerializeField] private ARSession arSession;
    
    private void Start()
    {
        // Check battery level periodically
        InvokeRepeating(nameof(CheckBatteryLevel), 30f, 30f);
    }
    
    private void CheckBatteryLevel()
    {
        float batteryLevel = SystemInfo.batteryLevel;
        
        if (batteryLevel < LOW_BATTERY_THRESHOLD)
        {
            // Warn user and suggest battery saver mode
            ShowBatteryWarning();
            
            // Offer to reduce AR features
            OfferBatterySaverMode();
        }
    }
    
    private void EnableBatterySaverMode()
    {
        // Reduce AR session update rate
        arSession.matchFrameRateEnabled = false;
        
        // Disable light estimation
        GetComponent<ARCameraManager>().requestedLightEstimation = 
            LightEstimation.None;
        
        // Reduce plane detection frequency
        // Disable non-essential AR features
        
        ShowMessage("Battery saver mode enabled");
    }
}
```

---

## Part IV: Cross-Platform Development Strategy

### 7. Abstraction Layer Pattern

**Platform-Agnostic AR Interface:**
```csharp
public interface IARPlatform
{
    bool IsSupported();
    void Initialize();
    void Shutdown();
    bool TryPlaceObject(Vector2 screenPoint, out Pose pose);
    void EnablePeopleOcclusion(bool enable);
    void HostCloudAnchor(ARAnchor anchor, Action<string> onComplete);
    void ResolveCloudAnchor(string anchorId, Action<ARAnchor> onComplete);
}

public class UnityARPlatform : IARPlatform
{
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    public bool IsSupported()
    {
        #if UNITY_IOS || UNITY_ANDROID
        return ARSession.state != ARSessionState.Unsupported;
        #else
        return false;
        #endif
    }
    
    public void Initialize()
    {
        // Platform-specific initialization
        #if UNITY_IOS
        InitializeARKit();
        #elif UNITY_ANDROID
        InitializeARCore();
        #endif
    }
    
    public bool TryPlaceObject(Vector2 screenPoint, out Pose pose)
    {
        pose = Pose.identity;
        
        if (raycastManager.Raycast(screenPoint, hits, TrackableType.PlaneWithinPolygon))
        {
            pose = hits[0].pose;
            return true;
        }
        
        return false;
    }
    
    // Implement other interface methods...
}
```

**Platform-Specific Compilation:**
```csharp
public class PlatformFeatures
{
    public static bool SupportsPeopleOcclusion()
    {
        #if UNITY_IOS
        return ARKitSession.supportsOcclusionPreference;
        #else
        return false;
        #endif
    }
    
    public static bool SupportsCloudAnchors()
    {
        #if UNITY_ANDROID
        return true; // ARCore supports cloud anchors
        #else
        return false;
        #endif
    }
    
    public static bool SupportsFaceTracking()
    {
        #if UNITY_IOS
        return ARKitFaceSubsystem.faceTrackingSupported;
        #else
        return false;
        #endif
    }
}
```

---

## Part V: Testing and Debugging

### 8. AR Testing Strategies

**Remote Testing (AR Remote):**
```csharp
public class ARRemoteTestManager : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] private bool useARRemote = true;
    
    private void Awake()
    {
        if (useARRemote)
        {
            // Enable AR Remote for testing in editor
            // Connects to companion app on device
            Debug.Log("AR Remote enabled for testing");
        }
    }
    #endif
}
```

**Simulation Mode (Unity Editor):**
```csharp
public class ARSimulation : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] private bool simulateARInEditor = true;
    [SerializeField] private GameObject[] simulatedPlanes;
    
    private void Start()
    {
        if (simulateARInEditor)
        {
            // Create simulated AR planes in editor
            foreach (GameObject plane in simulatedPlanes)
            {
                plane.SetActive(true);
            }
        }
    }
    #endif
}
```

---

## Part VI: BlueMarble Integration Recommendations

### 9. Implementation Roadmap

**Phase 1: Foundation (Months 1-2)**
```
- Set up Unity AR Foundation project
- Implement basic plane detection
- Create simple object placement
- Test on iOS and Android devices
- Establish performance baselines
```

**Phase 2: Core Features (Months 3-4)**
```
- Implement buildplate placement system
- Add light estimation for realistic rendering
- Create structure placement interface
- Implement scale switching (tabletop/life-size)
- Add screenshot/sharing functionality
```

**Phase 3: Advanced Features (Months 5-6)**
```
- Platform-specific optimizations
- People occlusion (iOS)
- Cloud anchors (Android) for sharing
- Multiplayer AR sessions (experimental)
- Integration with main game blueprint system
```

**Phase 4: Polish (Month 7+)**
```
- Performance optimization
- Battery management
- Extensive device testing
- User experience refinement
- Analytics integration
```

---

### 10. Key Technical Decisions

**Decision 1: Unity AR Foundation as Base**
- ✅ Cross-platform (iOS + Android from single codebase)
- ✅ Well-documented with community support
- ✅ Integrates with Unity ecosystem
- ❌ Abstraction layer may limit platform-specific features

**Decision 2: Conditional Compilation for Platform Features**
```csharp
#if UNITY_IOS
    // ARKit-specific features
#elif UNITY_ANDROID
    // ARCore-specific features
#else
    // Fallback or editor mode
#endif
```

**Decision 3: Performance Over Features**
- Prioritize 60 FPS over visual fidelity
- Implement dynamic quality scaling
- Limit AR session duration (5-10 minutes)
- Provide non-AR fallback mode

**Decision 4: Tabletop-First Design**
- Default to miniature scale (1:10)
- Life-size mode as optional showcase
- Simplifies controls and reduces motion sickness
- Better battery life

---

## Conclusion

Unity AR Foundation provides a solid cross-platform foundation for BlueMarble's AR features, with platform-specific 
optimizations available through ARKit (iOS) and ARCore (Android). The key to successful AR implementation is:

1. **Start Simple** - Basic plane detection and placement first
2. **Optimize Aggressively** - Mobile AR is resource-intensive
3. **Provide Fallbacks** - Not all devices support AR
4. **Limit Session Length** - Battery drain is a real concern
5. **Test Extensively** - AR behaves differently across devices

**Recommended Approach for BlueMarble:**
- Phase 1: 2D blueprint planning (no AR)
- Phase 2: AR visualization (view-only)
- Phase 3: AR building (simplified controls)
- Phase 4: Advanced features (if Phase 3 successful)

---

## References

### Unity AR Foundation
1. Unity AR Foundation Documentation - <https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest>
2. "Unity AR & VR by Tutorials" - raywenderlich.com
3. Unity AR Foundation Samples - GitHub Repository

### ARKit (iOS)
1. ARKit Documentation - <https://developer.apple.com/documentation/arkit>
2. "Building AR Experiences with ARKit" - Apple WWDC Sessions
3. ARKit Best Practices - Apple Developer Guidelines

### ARCore (Android)
1. ARCore Documentation - <https://developers.google.com/ar>
2. "ARCore Fundamentals" - Google Codelabs
3. ARCore Cloud Anchors Guide - Google AR Documentation

### Performance Optimization
1. "Mobile AR Performance Optimization" - Unity Blog
2. "Best Practices for AR" - Google AR Design Guidelines
3. "Power Management in AR Apps" - Mobile Development Guides

---

## Related Research

### Within BlueMarble Repository
- [game-dev-analysis-ar-concepts.md](game-dev-analysis-ar-concepts.md) - Parent AR research
- [game-dev-analysis-minecraft-earth-case-study.md](game-dev-analysis-minecraft-earth-case-study.md) - AR building patterns
- [research-assignment-group-18.md](research-assignment-group-18.md) - Discovery source

### Next Steps
- Prototype basic plane detection in Unity
- Test performance on target devices (iPhone 12+, Android flagship)
- Evaluate battery consumption in extended sessions
- Design simplified building controls for touch input

---

**Document Status:** Complete  
**Last Updated:** 2025-01-15  
**Next Steps:** Begin Unity AR Foundation prototype  
**Priority:** Medium - Technical foundation for Phase 2 AR features
