# Animator Factory

A Unity Editor tool for managing sprite animations, animator states, and prefab variants.

## Features
- Sprite sheet slicing and animation creation
- Animator state management
- Prefab variant generation

## Installation
### Via Package Manager (Git URL)
1. Open Package Manager (`Window > Package Manager`)
2. Click `+` → `Add package from git URL`
3. Enter: `https://github.com/kkosovsky/animator-factory.git` or `git@github.com:kkosovsky/animator-factory.git`

### Via Git Submodule
`https:`
```bash
git submodule add https://github.com/yourusername/AnimatorFactory.git Packages/com.kamilkosowski.animatorfactory
```
`ssh:`
```bash
git submodule add git@github.com:kkosovsky/animator-factory.git Packages/com.kamilkosowski.animatorfactory
```

`Then add to Packages/manifest.json:`
```json
"com.kamilkosowski.animatorfactory": "file:AnimatorFactory"
```


## Usage
1. Open `Window > Animator Factory`
2. Use the three tabs for different workflows:
   - **Sprite Edition**: Slice sprite sheets and create animations
   - **Animator States**: Manage animator states and transitions
   - **Prefab Variants**: Create and manage prefab variants