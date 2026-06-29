# SpellCraft Arena — Project Rules for Claude Sessions

## Tech Stack (observed, not assumed)

| Layer | Solution |
|---|---|
| Engine | Unity 6 (URP 17.4, UGUIv2) |
| Render | Universal Render Pipeline |
| Tweening | **DOTween** (`Assets/Plugins/Demigiant/`) — use for all animation/tween needs |
| Input | **Old `UnityEngine.Input`** in `DesktopInput.cs`; `com.unity.inputsystem` 1.19 is installed but unused; `SimpleInput` joystick plugin — `MobileInput.cs` impl exists; platform switch in `GameplayContextRegistrations` |
| DI / IoC | Custom hand-rolled DI via `DiContainer` and `Installer` base classes; no Zenject/VContainer |
| ECS | Custom entity-component system (NOT Unity DOTS); entities are C# objects holding typed component objects and lists of `IEntitySystem` |
| Reactive | Custom `ReactiveVariable<T>` (project-local, not UniRx/R3) |
| Camera | Cinemachine 3.1.4 |
| Navigation | `com.unity.ai.navigation` 2.0.13 (NavMesh) — used for enemy AI |
| UI | UGUI + TextMesh Pro; no UIToolkit |
| VFX | Particle Systems (Shuriken); Hovl Studio Magic Effects Pack; CFXR (Cartoon FX Remaster); project-local VFX under `Assets/_Project/Art/VFX/` |
| 3D Models | LowPoly characters; Cartoon/Stylized weapons and staffs; PBR wizard character |
| Configs | ScriptableObjects loaded via `Resources.Load`; paths are relative to `Assets/_Project/Resources/` or `Assets/Resources/` |
| Tests | `com.unity.test-framework` 1.6.0 |
| MCP | `com.coplaydev.unity-mcp` — connected and active |

## Architecture Patterns

### Custom ECS

- Entities: plain C# class `Entity` holding a typed dictionary of `IEntityComponent` instances.
- Components: plain C# classes, NOT MonoBehaviours. Reactive state lives in `ReactiveVariable<T>` fields on components.
- Systems: implement `IEntitySystem` with `Update(Entity)`. Registered on the entity at construction time. Systems are NOT MonoBehaviours.
- MonoBehaviour views (`EntityView` subclasses) are attached to the entity's root GameObject and bridge the entity to Unity (rendering, physics, UI world-space).
- Entry point: `GameplayBootstrap` wires up all contexts, presenters, and factory calls via the DI container.

### AI / State Machine

- `AIStateMachine` with typed states. States implement `IState` (or `IAIState`) with `Enter`, `Update`, `Exit`.
- `AIParallelState` runs multiple sub-state-machines in parallel.
- All hero/enemy brains are built in `BrainsFactory`.
- `FindTargetState` + `NearestDamagableTargetSelector` run every frame scanning `EntitiesLifeContext.Entities`.

### Factory Pattern

- `EntitiesFactory` creates all gameplay entities programmatically (hero, enemies, projectiles).
- Prefabs for entities live in `Assets/_Project/Resources/Entities/` and are loaded by string path via `MonoEntitiesFactory.Create(entity, position, "Entities/PrefabName")`.

### Config / ScriptableObject Conventions

- All runtime-loaded configs: `Assets/_Project/Resources/Configs/` (and its subfolders).
- Access pattern: `ConfigsProviderService.GetConfig<T>(id)`.
- Base class: `AbilityConfig : ScriptableObject` (fields: `ID`, `Name`, `Description`, `Icon : Sprite`, `MaxLevel`).
- Container assets (lists of configs): e.g. `AbilitiesConfigsContainer.asset`.
- DO NOT put new configs outside `Resources/Configs/` — they won't be found by the provider.

### Reactive State Convention

- Entity reactive state: `entity.SomeComponent.Value` (a `ReactiveVariable<T>`).
- Subscribe: `reactiveVar.Subscribe(callback)` — always unsubscribe on entity release. **Signature**: `Subscribe(Action<T, T>)` — first arg is prev value, second is next.
- Events (fire-once pulses): `ReactiveEvent` — subscribe with `Action` (no args), fire with `Invoke()`.

### Spell Casting System (Magicka/Invoker style)

- **7 Aspects** (enum `Aspect`): `Blood(0), Fire(1), Light(2), Death(3), Nature(4), Ice(5), Magic(6)`.
- Player selects 3 aspects via `SpellPanelView` buttons → `SpellsConfigsContainer.FindByAspects` (multiset, order-independent) resolves the spell.
- Resolved spell stored in `entity.ActiveSpellConfig` (`ActiveSpellConfig` component).
- Cast triggered by `entity.CastRequest` (a `ReactiveEvent`): desktop = Space key; mobile = UI button.
- **Windup**: `SpellCastingWindupSystem` sets `IsCasting = true`, ticks `CastWindupCurrentTime`; when elapsed ≥ `config.CastTime` (0.4s default), executes and resets.
- **Movement blocked** during windup: `IsCasting.Value == false` is a `FuncCondition` in `canMove` (`ICompositeCondition` on the hero entity).
- `SpellConfig` fields: `ID, Name, Icon, Aspects[3], ManaCost, CastTime, CastType (Projectile/AoE), PrefabPath, Damage, AoeRadius`.
- New spells: add `SpellConfig.asset` to `Resources/Configs/Gameplay/Spells/`, add to `SpellsConfigsContainer.Spells` list — no code changes needed for the Projectile/AoE routing.
- `EntityAPIGenerator` auto-generates `EntityAPI.cs` on every domain reload — no manual edits needed to that file after first compile.

## Project Folder Structure

```
Assets/
  _Project/                     <- ALL game code and first-party art
    Art/
      Animations/
      Fonts/
      Materials/                (Coin.mat, Exp.mat, Loot.mat)
      Models/
      SFX/
      Sprites/UI/               (health_potion, mana_potion, rune circles)
      VFX/                      (CurrentTargetVFX, FrostStrikeVFX, HIT_*, MagicBarrier, Spawn VFXs)
    Develop/
      Runtime/
        Configs/                (ScriptableObject C# definitions)
        Gameplay/
          Common/               (shared utilities)
          EntitiesCore/         (Entity, IEntityComponent, IEntitySystem, EntitiesFactory, etc.)
          Features/             (one subfolder per gameplay feature)
            AI/
            ApplyDamage/
            Attack/
            InputFeature/
            LifeCycle/
            Loot/
            ManaFeature/          (CurrentMana, MaxMana, ManaRegenRate, ManaRegenSystem)
            Movement/
            Spawner/
            SpellcastingFeature/  (SpellcastingComponents, SpellCastingWindupSystem, OrbsDisplayView)
            ...
          Infrastructure/
          States/
        Infrastructure/
        Meta/
        UI/
        Utilites/
      Editor/
    Resources/                  <- loaded at runtime via Resources.Load
      Configs/
        Entities/               (HeroConfig.asset, GhostConfig.asset)
        Gameplay/
          Abilities/            (AbilityConfig assets + AbilitiesConfigsContainer.asset)
          Levels/
          Loot/
          Spells/               (SpellConfig assets + SpellsConfigsContainer.asset)
          Stages/
        Meta/
          Stats/
          Wallet/
      Entities/                 (Hero.prefab, Ghost.prefab, FireballProjectile.prefab)
      Prefabs/
        Spells/                 (FireballProjectile.prefab, FrostStrikeSpell.prefab)
      UI/
        Gameplay/               (GameplayScreenView.prefab, GameplayUIRoot.prefab, ManaBar.prefab, HealthBars/, SpellPanel/)
        MainMenu/
        ...
    Scenes/                     (Gameplay.unity, MainMenu.unity, GameEntryPoint.unity)
    Settings/                   (URP pipeline assets)

  Plugins/
    Demigiant/                  (DOTween)
    SimpleInput/                (joystick input, .asmdef: SimpleInput.Runtime)

  Hovl Studio/Magic effects pack/Prefabs/   <- VFX library
  Fireballs/                                <- Fireball VFX pack (01-05)
  3D Items - Wand Pack/Prefabs/             <- orb01-05 × 5 colors, wand01-05 × 5 colors
  Blink/Art/Icons/Classes/                  <- class/aspect icons (Pyromancer, Cryomancer, Arcanist, etc.)
  RGP icons free start pack/items/          <- generic RPG icons (book, shield, scroll, etc.)
  GUI Pro-FantasyRPG/                       <- UI component prefabs
  GUI_Parts/                                <- HP bars, skill slot icons
  WizardPBR/                                <- PBR wizard character + staves
  Cartoon_Weapon_Pack/                      <- Staff_01..06 + misc weapons
  JMO Assets/Cartoon FX Remaster/          <- CFXR particle system pack
```

## Key File Locations (Critical Paths)

| What | Path |
|---|---|
| Hero entity creation | `_Project/Develop/Runtime/Gameplay/EntitiesCore/EntitiesFactory.cs` |
| Hero brain (state machine) | `_Project/Develop/Runtime/Gameplay/Features/AI/BrainsFactory.cs` |
| Auto-attack systems | `_Project/Develop/Runtime/Gameplay/Features/Attack/` |
| Targeting | `_Project/Develop/Runtime/Gameplay/Features/AI/States/NearestDamagableTargetSelector.cs` |
| Input interface | `_Project/Develop/Runtime/Gameplay/Features/InputFeature/IInputService.cs` |
| Desktop input impl | `_Project/Develop/Runtime/Gameplay/Features/InputFeature/DesktopInput.cs` |
| Mobile input impl | `_Project/Develop/Runtime/Gameplay/Features/InputFeature/MobileInput.cs` |
| Fireball spawn | `EntitiesFactory.CreateFireballProjectile()` |
| Multi-direction shoot | `_Project/Develop/Runtime/Gameplay/Features/Attack/DirectionsInstantShootSystem.cs` |
| Health/death components | `_Project/Develop/Runtime/Gameplay/Features/LifeCycle/LifeCycleComponents.cs` |
| Mana components | `_Project/Develop/Runtime/Gameplay/Features/ManaFeature/ManaComponents.cs` |
| Mana regen system | `_Project/Develop/Runtime/Gameplay/Features/ManaFeature/ManaRegenSystem.cs` |
| Spell cast system | `_Project/Develop/Runtime/Gameplay/Features/SpellcastingFeature/SpellCastingWindupSystem.cs` |
| Spell components | `_Project/Develop/Runtime/Gameplay/Features/SpellcastingFeature/SpellcastingComponents.cs` |
| Orb display (EntityView) | `_Project/Develop/Runtime/Gameplay/Features/SpellcastingFeature/OrbsDisplayView.cs` |
| Cast animation (EntityView) | `_Project/Develop/Runtime/Gameplay/Features/SpellcastingFeature/SpellCastView.cs` |
| Spell configs | `_Project/Resources/Configs/Gameplay/Spells/` (FireballSpell.asset, IceSpikesSpell.asset) |
| Spell container | `_Project/Resources/Configs/Gameplay/Spells/SpellsConfigsContainer.asset` |
| Spell panel UI | `_Project/Develop/Runtime/UI/Gameplay/SpellPanel/` |
| Spell panel prefab | `_Project/Resources/UI/Gameplay/MagicViews/CombatActionsBarView.prefab` (has SpellPanelView + 7x AspectButtonView) |
| Mana bar presenter | `_Project/Develop/Runtime/UI/Gameplay/ManaDisplay/ManaBarPresenter.cs` |
| Gameplay HUD | `_Project/Develop/Runtime/UI/Gameplay/GameplayScreenView.cs` |
| Gameplay HUD presenter | `_Project/Develop/Runtime/UI/Gameplay/GameplayScreenPresenter.cs` (LateUpdate moves mana bar) |
| Ability configs | `_Project/Resources/Configs/Gameplay/Abilities/` |
| Entity prefabs | `_Project/Resources/Entities/` |
| Spell prefabs | `_Project/Resources/Prefabs/Spells/` |

## What Exists vs. What Doesn't

### Implemented and working
- Hero auto-attack **disabled** — replaced by manual spell casting (auto-attack code kept in `BrainsFactory.CreateAutoAttackStateMachine` but never called)
- Nearest-enemy targeting per frame (still used by auto-attack if re-enabled)
- Attack cooldown/delay pipeline (7 systems in order) — still intact under `Features/Attack/`
- Stats system (4 stats: MoveSpeed, MaxHealth, Damage, AttacksPerSecond)
- Health display (world-space bars, color-coded, reactive)
- Level-up and ability-select popup
- Meta shop (stat upgrades)
- Loot drops (coins, XP, health)
- Enemy AI (Ghost: NavMesh wander)
- Bounce projectile ability
- **Mana system**: `CurrentMana`, `MaxMana`, `ManaRegenRate` components; `ManaRegenSystem` regenerates mana per second; blue mana bar in HUD (`ManaBarPresenter`)
- **Aspect combo spell system**: 7 aspects (`Blood, Fire, Light, Death, Nature, Ice, Magic`); pick 3 to form a combo; `SpellsConfigsContainer.FindByAspects` does multiset (order-independent) matching
  - Fireball = Fire + Magic + Light (Projectile, 25 mana, 50 dmg)
  - Ice Spikes = Ice + Nature + Death (AoE r=3, 35 mana, 60 dmg, uses FrostStrikeSpell VFX)
- **SpellCastingWindupSystem**: 0.4s windup, blocks movement (`IsCasting` in `canMove` condition), deducts mana, routes to projectile or AoE logic
- **SpellPanelView** (HUD): backed by `CombatActionsBarView.prefab` (nested in `GameplayScreenView.prefab`); 7 `AspectButtonView` components (short tap = add, long press ≥0.35s = remove), cast button (`AttackButton`), teleport/shield stub buttons; `OrbsDisplayView` on Hero prefab shows selected aspect orbs in world space
  - Orb prefab mapping (index = Aspect enum): [0]=Blood→orb01_red, [1]=Fire→orb02_red, [2]=Light→orb01_yellow, [3]=Death→orb01_purple, [4]=Nature→orb01_green, [5]=Ice→orb01_blue, [6]=Magic→orb02_purple
- **Desktop cast**: Space key — `PlayerInputMovementState.Update()` calls `entity.CastRequest.Invoke()` when `_inputService.IsCastRequested`; **Mobile cast**: UI cast button in `SpellPanelView` fires same event
- **MobileInput.cs**: `SimpleInput.GetAxis("Horizontal/Vertical")`, `IsCastRequested = false`; platform selected in `GameplayContextRegistrations`
- **Cast animation**: `SpellCastView` (EntityView on HeroView GO) subscribes to `entity.IsCasting` → sets `Animator.SetBool("IsAttacking", value)`, reusing the existing attack animation
- **Mana bar position**: follows hero in world space — `GameplayScreenPresenter.LateUpdate()` projects hero's `HealthBarPoint` to screen coords and offsets `ManaBarView` 30px below

### Stubs / VFX only, no code
- `FrostStrikeSpell.prefab` — used as AoE VFX for Ice Spikes (loaded at runtime via `Resources.Load`)
- `BlinkButton.prefab` — UI stub only (logs `Debug.Log` on press)
- `PotionsView.prefab` — UI stub only

### Not present at all
- Teleport / blink mechanic (code) — stub button logs `Debug.Log`
- Shield mechanic (code) — stub button logs `Debug.Log`
- Blood aspect icon (closest: Cultist icon used as placeholder)
- SimpleInput joystick prefab not yet added to Gameplay scene (needed for mobile movement)

## Coding Conventions (observed)

1. **New feature = new folder under `Features/`** with its own systems, states, and components files.
2. **Components are C# classes** in a single `*Components.cs` file per feature (e.g. `LifeCycleComponents.cs`).
3. **Systems are C# classes** implementing `IEntitySystem`, named `*System.cs`.
4. **One class per file** is the norm; group only when tightly coupled (e.g. component definitions).
5. **Prefab load paths are string literals** passed to `MonoEntitiesFactory.Create()` — keep them in sync with actual `Resources/` subfolder structure.
6. **ScriptableObject configs** go in `Resources/Configs/`, are accessed via `ConfigsProviderService`, never via direct `AssetDatabase` calls at runtime.
7. **No Addressables** — everything runtime-loaded is in `Resources/`.
8. **No Zenject/VContainer** — use the project's own `DiContainer` and `Installer` pattern.
9. **No UniRx** — use the project's own `ReactiveVariable<T>`.
10. **No DOTS/Burst** — custom ECS only.
11. **No `Assembly-CSharp-firstpass`** — main game code has no `.asmdef`, compiles into `Assembly-CSharp`.

## VFX Assets Quick Reference (for spell implementation)

| Effect | Path |
|---|---|
| Fireball VFX (01-05 variants) | `Fireballs/Fireball 0X.prefab` |
| Ice / Frost strike | `_Project/Art/VFX/FrostStrikeVFX.prefab` |
| Snow AOE | `Hovl Studio/Magic effects pack/Prefabs/AoE effects/Snow AOE.prefab` |
| Snow hit | `Hovl Studio/Magic effects pack/Prefabs/Hits and explosions/Snow hit.prefab` |
| Magic shields | `Hovl Studio/Magic effects pack/Prefabs/Magic shields/` (blue/pink/yellow) |
| MagicBarrier (project) | `_Project/Art/VFX/MagicBarrier.prefab` |
| Teleport | `Hovl Studio/Magic effects pack/Prefabs/Environment/Teleport.prefab` |
| Portals (4 colors) | `Hovl Studio/Magic effects pack/Prefabs/Portals/` |
| Character auras | `Hovl Studio/Magic effects pack/Prefabs/Character auras/` |
| Crystals front | `Hovl Studio/Magic effects pack/Prefabs/AoE effects/Crystals front attack.prefab` |
| Magic circles | `Hovl Studio/Magic effects pack/Prefabs/Magic circles/` |
| Orbs 3D (25 variants) | `3D Items - Wand Pack/Prefabs/orb0X (color).prefab` |

## Aspect/Class Icons Quick Reference

| Aspect | Closest existing icon folder |
|---|---|
| Fire | `Blink/Art/Icons/Classes/Elementalist/Pyromancer/` |
| Ice | `Blink/Art/Icons/Classes/Elementalist/Cryomancer/` |
| Magic / Arcane | `Blink/Art/Icons/Classes/Elementalist/Arcanist/` |
| Death | `Blink/Art/Icons/Classes/HolyDarkness/Necromancer/` |
| Light | `Blink/Art/Icons/Classes/HolyDarkness/Paladin/` |
| Nature | `Blink/Art/Icons/Classes/Symbiose/Druid/` |
| Blood | **No dedicated icon** — closest: `Blink/Art/Icons/Classes/HolyDarkness/Cultist/` |

Action icons (book, shield, scroll): `RGP icons free start pack/items/`
Generic skill slots (4 icons): `GUI_Parts/Icons/skill_icon_0X_nobg.png`
