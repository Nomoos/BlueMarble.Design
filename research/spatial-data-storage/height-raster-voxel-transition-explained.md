# Přechod na Voxely - Detailní Vysvětlení / Voxel Transition - Detailed Explanation

## Obsah / Contents

1. [Co znamená přechod na voxely?](#1-co-znamená-přechod-na-voxely)
2. [Datová reprezentace](#2-datová-reprezentace)
3. [Kdy dochází k přechodu?](#3-kdy-dochází-k-přechodu)
4. [Praktické příklady](#4-praktické-příklady)

---

## 1. Co znamená přechod na voxely?

### 1.1 Základní Princip

**Height Raster** (výškový rastr) může ukládat pouze **jednu výšku** pro každou XY pozici:

```
Normální terén (height raster funguje):
    
    ##########   ← Jeden vrchol pro každé X,Y
    ##      ##
    ##        ##
    Ground

Height raster: h[x,y] = jedna hodnota
```

**Přesah** (overhang) potřebuje **více výšek** na stejné XY pozici:

```
Přesah (height raster NELZE použít):

    ######**     ← Dvě výšky na stejné X pozici!
    ##    **     ← Horní část + spodní část
    ##      **
    ##
    Ground

Potřeba voxel storage: více hodnot na [x,y]
```

### 1.2 Co to znamená v praxi?

**Před přechodem** (height raster):
- Data: Pouze 2D pole výšek `float[x,y]`
- Materiály: 8 vrstev od povrchu
- Velikost: ~24 MB/km²

**Po přechodu** (voxel region):
- Data: 3D pole voxelů `byte[x,y,z]`
- Materiály: Každý voxel má svůj materiál
- Velikost: +50-180 MB/km² (závisí na hustotě)

---

## 2. Datová Reprezentace

### 2.1 Height Raster Data (běžný terén)

```csharp
public class HeightRasterTile
{
    // Hlavní data - 2D pole výšek
    public float[] Heights;  // [TILE_SIZE × TILE_SIZE]
    
    // Materiály v 8 vrstvách od povrchu
    public MaterialColumn[] MaterialColumns;  // [TILE_SIZE × TILE_SIZE]
    
    // Každá pozice [x,y] má:
    // 1. Výška: heights[y * TILE_SIZE + x] = např. 45.7m
    // 2. Materiály: 8 vrstev (0m, 0.25m, 0.5m, 1m, 2m, 5m, 10m, 20m pod povrchem)
}

// Příklad pro pozici [100, 200]:
Heights[200 * 1024 + 100] = 45.7f  // Výška terénu
MaterialColumns[200 * 1024 + 100] = {
    Layer[0] = Grass     // Povrch (45.7m)
    Layer[1] = Soil      // 45.45m (0.25m pod povrchem)
    Layer[2] = Soil      // 45.2m
    Layer[3] = Soil      // 44.7m
    Layer[4] = Rock      // 43.7m
    Layer[5] = Rock      // 40.7m
    Layer[6] = Bedrock   // 35.7m
    Layer[7] = Bedrock   // 25.7m
}
// Hlouběji než 20m → procedurální generování
```

**Velikost dat:**
```
TILE_SIZE = 1024 (pokrývá 256m × 256m při 0.25m/voxel)

Heights: 1024 × 1024 × 4 bytes = 4 MB
MaterialColumns: 1024 × 1024 × 8 bytes = 8 MB
Total per tile: ~12 MB
```

### 2.2 Voxel Region Data (sráz/přesah)

```csharp
public class VoxelColumn
{
    // 3D sparse dictionary - ukládá pouze obsazené voxely
    private Dictionary<int, byte> _voxels;  // [Z coordinate → material]
    
    private Vector2Int _cellCoordinate;  // XY pozice v rasteru
    private float _baseHeight;
    
    // Každý voxel může mít jiný materiál
    // Příklad pro pozici [100, 200] se srázem:
    _voxels = {
        [180] = Air,      // 45.0m (180 × 0.25m)
        [179] = Air,      // 44.75m
        [178] = Rock,     // 44.5m ← Vrchol srázu
        [177] = Rock,     // 44.25m
        [176] = Rock,     // 44.0m
        // ... střední část je prázdná (vzduch) kvůli převisu ...
        [150] = Rock,     // 37.5m ← Spodní část převisu
        [149] = Rock,     // 37.25m
        [148] = Rock,     // 37.0m
    }
}
```

**Velikost dat:**
```
Voxel Column pro sráz 50m vysoký:
- Voxely: ~200 voxelů × (12 bytes overhead + 1 byte material) = 2.6 KB
- Pro celou oblast 10×10 buněk se srázy: 10 × 10 × 2.6 KB = 260 KB

Srovnání:
- Height raster region (10×10): 10 × 10 × 12 bytes = 1.2 KB
- Voxel region (10×10): ~260 KB (217x více)
- Ale: Voxel region umí přesahy, height raster ne!
```

### 2.3 Hybridní Struktura

```csharp
public class HybridHeightRasterVoxel
{
    // Základní terén - height raster (většina dat)
    private HeightRasterTile _heightRaster;  // ~12 MB pro 256m × 256m
    
    // Speciální oblasti - voxel regions (jen kde potřeba)
    private Dictionary<Vector2Int, VoxelColumn> _voxelRegions;
    
    // Delta overlay - uživatelské změny
    private DeltaOverlay _userModifications;
    
    public byte GetMaterial(Vector3 worldPos)
    {
        Vector2Int cellCoord = WorldToCell(worldPos.XY);
        
        // Priorita dotazování:
        
        // 1. NEJPRVE: Uživatelské změny (delta overlay)
        if (_userModifications.TryGetModification(worldPos, out byte userMat))
            return userMat;  // Hráč postavil něco
        
        // 2. DRUHÉ: Voxel regions (srázy, jeskyně)
        if (_voxelRegions.TryGetValue(cellCoord, out var voxelColumn))
            return voxelColumn.GetMaterial(worldPos);  // Oblast má přesah
        
        // 3. POSLEDNÍ: Height raster (normální terén)
        float surfaceHeight = _heightRaster.GetHeight(worldPos.XY);
        float depthBelowSurface = surfaceHeight - worldPos.Z;
        
        if (depthBelowSurface < 0)
            return MaterialId.Air;  // Nad povrchem
        else
            return _heightRaster.GetMaterialAtDepth(cellCoord, depthBelowSurface);
    }
}
```

---

## 3. Kdy dochází k přechodu?

### 3.1 Detekce Sklonu

**Pravidlo**: Když **rozdíl výšky mezi sousedními buňkami** je větší než určitý práh.

```csharp
public class CliffDetector
{
    private const float CELL_SIZE = 0.25f;  // Velikost buňky
    private const float VERTICAL_CLIFF_THRESHOLD = 70.0f;  // stupně
    
    public bool ShouldConvertToVoxel(HeightRasterTile tile, int x, int y)
    {
        float h0 = tile.Heights[y * TILE_SIZE + x];        // Aktuální výška
        float h1 = tile.Heights[y * TILE_SIZE + (x + 1)];  // Soused vpravo
        float h2 = tile.Heights[(y + 1) * TILE_SIZE + x];  // Soused dole
        
        // Vypočítej sklon v obou směrech
        float slopeX = (h1 - h0) / CELL_SIZE;  // Změna výšky na vzdálenost
        float slopeY = (h2 - h0) / CELL_SIZE;
        
        // Převeď na úhel
        float angleX = Math.Atan(slopeX) * 180.0f / Math.PI;
        float angleY = Math.Atan(slopeY) * 180.0f / Math.PI;
        
        float maxAngle = Math.Max(Math.Abs(angleX), Math.Abs(angleY));
        
        return maxAngle > VERTICAL_CLIFF_THRESHOLD;  // > 70° = voxel
    }
}
```

### 3.2 Výpočet Rozdílu Výšky

**Vztah mezi rozdílem výšky a sklonem:**

```
Velikost buňky (CELL_SIZE) = 0.25m
Rozdíl výšky (ΔH) mezi sousedy:

Sklon = ΔH / 0.25m
Úhel = arctan(Sklon) × 180° / π

Příklady:
┌─────────┬─────────┬────────┬──────────────┐
│ ΔH      │ Sklon   │ Úhel   │ Reprezentace │
├─────────┼─────────┼────────┼──────────────┤
│ 0.1m    │ 0.4     │ 22°    │ Height Raster│
│ 0.25m   │ 1.0     │ 45°    │ Height Raster│
│ 0.5m    │ 2.0     │ 63°    │ Height Raster│
│ 0.7m    │ 2.8     │ 70°    │ PRÁH!        │
│ 1.0m    │ 4.0     │ 76°    │ Voxel Region │
│ 2.0m    │ 8.0     │ 83°    │ Voxel Region │
│ 5.0m    │ 20.0    │ 87°    │ Voxel Region │
└─────────┴─────────┴────────┴──────────────┘
```

### 3.3 Odpověď na Otázku: "8 bloků rozdíl = voxel?"

**ANO, přibližně!**

```
Při CELL_SIZE = 0.25m:
- 1 blok = 0.25m
- 8 bloků = 8 × 0.25m = 2.0m rozdíl výšky

Výpočet:
- Sklon = 2.0m / 0.25m = 8.0
- Úhel = arctan(8.0) = 83°

83° > 70° (práh) → ANO, převést na voxel!
```

**Obecně:**

```csharp
// Minimální rozdíl výšky pro voxel konverzi:
float minHeightDiff = CELL_SIZE * Math.Tan(70° * Math.PI / 180.0);
// = 0.25m × 2.747 = 0.687m

// Kolik bloků to je?
int minBlockDiff = (int)Math.Ceiling(minHeightDiff / CELL_SIZE);
// = 0.687m / 0.25m = 2.75 → 3 bloky

// Odpověď:
// - 3 bloky (0.75m): 70° - na prahu
// - 8 bloků (2.0m): 83° - určitě voxel
// - 12 bloků (3.0m): 85° - výrazný sráz
```

---

## 4. Praktické Příklady

### 4.1 Příklad 1: Mírný Svah (Height Raster Stačí)

```
Terén profil:
Z ↑
  │
15│                    ####
  │                ####
14│            ####
  │        ####
13│    ####
  │####
12└────────────────────────────→ X
   0   1   2   3   4   5

Výšky:
h[0] = 12.0m
h[1] = 12.5m  ← Δ = 0.5m
h[2] = 13.0m  ← Δ = 0.5m
h[3] = 13.5m  ← Δ = 0.5m
h[4] = 14.0m  ← Δ = 0.5m
h[5] = 15.0m  ← Δ = 1.0m

Analýza:
- Max rozdíl: 1.0m mezi sousedy
- Sklon: 1.0m / 0.25m = 4.0
- Úhel: arctan(4.0) = 76°

76° > 70° → Poslední úsek by se mohl převést na voxel
Ale: 0.5m rozdíly → 63° → Height raster OK pro většinu
```

**Uloženo v:**
- Height Raster: Všechny výšky (24 bytes celkem)
- Materiály: 8 vrstev pro každou pozici
- Voxel Regions: Možná poslední buňka

### 4.2 Příklad 2: Strmý Sráz (Nutný Voxel)

```
Terén profil s převisem:
Z ↑
  │    ######**
25│    ##    **  ← Převis!
  │    ##      **
24│    ##        **
  │    ##
23│    ##
  │    ##
22│    ##
  │####
12└────────────────────────────→ X
   0   1   2   3   4   5

Height raster nemůže reprezentovat!
h[2] = 12.0m (spodek)
h[2] = 25.0m (vrchol) ← DvĚ hodnoty pro stejné X,Y!

Řešení: Voxel Column pro x=2:
_voxels = {
    [100] = Rock,  // 25.0m (vrchol)
    [99]  = Rock,  // 24.75m
    ...
    [92]  = Rock,  // 23.0m
    [91]  = Air,   // 22.75m ← Převis začíná
    [90]  = Air,   // 22.5m
    ...
    [49]  = Air,   // 12.25m
    [48]  = Rock,  // 12.0m (spodek)
}
```

**Uloženo v:**
- Height Raster: h[2] = 25.0m (vrchol)
- Voxel Region: Kompletní 3D reprezentace pro x=2
- Velikost: ~400 bytes pro tuto pozici

### 4.3 Příklad 3: Hráčem Postavený Sloup Zasypán

```
Situace:
1. Hráč postaví sloup:
   
   ####  ← Z=15m (top)
   ####  ← Z=14m
   ####  ← Z=13m
   ####  ← Z=12m
   Ground (Z=10m)

2. Terén sesune a zasype sloup:
   
   ########  ← Nový povrch Z=17m
   ########
   ####****  ← Sloup skrytý uvnitř
   ####****
   ########
   Ground (Z=10m)

Dotaz: Co je v rasteru? Co je voxel?
```

**Odpověď:**

```csharp
// 1. HEIGHT RASTER obsahuje:
Heights[x,y] = 17.0m  // Nový povrch po sesuvu

MaterialColumns[x,y] = {
    Layer[0] = Soil,     // 17.0m (nový povrch)
    Layer[1] = Soil,     // 16.75m
    Layer[2] = Soil,     // 16.5m
    Layer[3] = Soil,     // 16.0m
    Layer[4] = Soil,     // 15.0m
    Layer[5] = Soil,     // 12.0m
    Layer[6] = Rock,     // 7.0m
    Layer[7] = Rock,     // -3.0m (pod původním povrchem)
}

// 2. DELTA OVERLAY obsahuje (uživatelské bloky):
_modifications = {
    Vector3(x, y, 12) = Stone,  // Z=12m
    Vector3(x, y, 13) = Stone,  // Z=13m
    Vector3(x, y, 14) = Stone,  // Z=14m
    Vector3(x, y, 15) = Stone,  // Z=15m
}

// Metadata:
IsVisible = false  // Bloky jsou skryté
PlacedTime = (čas umístění)
LastHidden = (čas zasypání)

// 3. VOXEL REGIONS:
// Žádné! Není potřeba, sloup je v delta overlay

// Dotaz na materiál v Z=13m:
public byte GetMaterial(Vector3(x, y, 13))
{
    // Priorita 1: Delta overlay
    if (_userModifications.TryGet((x,y,13), out byte mat))
        return mat;  // → Vrátí Stone (hráčův sloup)
    
    // Priorita 2 & 3 by vrátily Soil (zasypací materiál)
}
```

**Ukládání:**
```
Sloup zůstává v Delta Overlay:
- Hot Tier (RAM): NE (není viditelný)
- Warm Tier (SSD): ANO (7 dní od zasypání)
- Cold Tier (Archive): ANO (po 7 dnech)

Velikost:
- 4 bloky × 25 bytes = 100 bytes (metadata + pozice + materiál)

Pokud hráč vykope terén zpět:
- Bloky se stávají viditelnými
- Přesunou se do Hot Tier (RAM)
- Renderují se normálně
```

### 4.4 Příklad 4: Postupná Konverze při Kopání

```
Scénář: Hráč kope tunel do kopce

Krok 1: Normální kopec (height raster):
Z ↑
  │    ########
30│    ########
  │    ########
20│    ########
  │####
10└────────────────────→ X
   0   1   2   3

Heights: [10, 20, 30, 30, 30, ...]
No voxels needed.

Krok 2: Začátek tunelu (stále height raster OK):
Z ↑
  │    ########
30│    ##    ##  ← Hráč odstranil vrchol
  │    ########
20│    ########
  │####
10└────────────────────→ X

Heights: [10, 20, 25, 30, 30, ...]
User modifications: (x=2, z=30) = Air, (x=2, z=29) = Air, ...
Stále žádné voxels.

Krok 3: Hluboký tunel (POTŘEBA VOXEL):
Z ↑
  │    ########
30│    ##    ##
  │    ##    ##  ← Tunel hluboko uvnitř
20│    ##    ##
  │####      ##
10└────────────────────→ X

Nyní x=2 má:
- Povrch v Z=30m
- Tunel v Z=20m
- Terén pod tunelem

→ PŘEVIS! Height raster nemůže reprezentovat
→ Konverze na voxel region pro x=2
```

**Proces konverze:**

```csharp
// Detekce potřeby konverze:
if (HasOverhang(cellCoord))  // Detekuje: terén nad + pod vzduchovou mezerou
{
    ConvertToVoxelRegion(cellCoord);
}

void ConvertToVoxelRegion(Vector2Int cellCoord)
{
    // 1. Vytvoř voxel column
    var voxelColumn = new VoxelColumn();
    
    // 2. Přenese data z height raster:
    float surfaceHeight = _heightRaster.GetHeight(cellCoord);
    for (int z = 0; z < 100; z++)
    {
        float worldZ = surfaceHeight - z * CELL_SIZE;
        byte material = _heightRaster.GetMaterialAtDepth(worldZ);
        if (material != Air)
            voxelColumn.SetVoxel(z, material);
    }
    
    // 3. Přenes uživatelské změny:
    foreach (var mod in _userModifications.GetInColumn(cellCoord))
    {
        voxelColumn.SetVoxel(mod.Position.Z, mod.Material);
    }
    
    // 4. Ulož voxel region:
    _voxelRegions[cellCoord] = voxelColumn;
    
    // 5. Height raster data zůstává (pro okolní buňky a LOD)
}
```

---

## 5. Shrnutí / Summary

### 5.1 Kdy je co použito?

| Situace | Reprezentace | Velikost | Proč |
|---------|--------------|----------|------|
| **Rovinatý terén** | Height Raster | 24 MB/km² | Jednoduchý povrch |
| **Kopcovitý terén** | Height Raster | 24 MB/km² | Stále jednoduchý povrch |
| **Srázy <70°** | Height Raster | 24 MB/km² | Interpolace stačí |
| **Srázy >70°** | Voxel Regions | +50 MB/km² | Příliš strmé |
| **Převisy** | Voxel Regions | +180 MB/km² | Více výšek na XY |
| **Jeskyně** | Voxel Regions | +300 MB/km² | Podzemní dutiny |
| **Hráčské bloky (viditelné)** | Delta Overlay (Hot) | ~0.1% dat | RAM pro render |
| **Hráčské bloky (skryté)** | Delta Overlay (Warm/Cold) | ~5% dat | SSD/archive |

### 5.2 Co je v Rasteru?

**Height Raster obsahuje:**
```
1. Výšky povrchu (float per XY)
   - Interpolované (bilineární)
   - Nejrychlejší přístup
   
2. Materiálové vrstvy (8 vrstev od povrchu)
   - 0m, 0.25m, 0.5m, 1m, 2m, 5m, 10m, 20m
   - Pod 20m → procedurální
   
3. NIC JINÉHO
   - Ne uživatelské změny
   - Ne srázy >70°
   - Ne převisy
   - Ne jeskyně
```

### 5.3 Práh 8 Bloků

**Odpověď na původní otázku:**

```
Při CELL_SIZE = 0.25m a THRESHOLD = 70°:

Minimální rozdíl: 3 bloky (0.75m) → 70°
Doporučený rozdíl: 8+ bloků (2.0m+) → 83°+ pro voxel

Proč ne přesně?
- 70° je minimální práh (začátek problémů)
- 75-80° je praktický práh (viditelné artefakty)
- 85°+ je jasně potřeba voxel

8 bloků je dobrá heuristika pro automatickou konverzi.
```

### 5.4 Výhody Hybridního Přístupu

✅ **Height Raster pro 95% terénu**
- Velmi rychlé dotazy
- Minimální paměť
- Jednoduché operace

✅ **Voxel Regions pro 5% terénu**
- Plná 3D reprezentace
- Podporuje jakoukoliv geometrii
- Automatická detekce kdy potřeba

✅ **Delta Overlay pro uživatelské změny**
- Oddělená od terénu
- Efektivní ukládání
- Tiered storage (hot/warm/cold)

**Výsledek:**
- 95% storage reduction oproti full 3D
- 5x rychlejší dotazy
- Podporuje VŠE (terén i srázy i uživatelské bloky)
