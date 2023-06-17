# Tags Layers SortingLayer: Generator
Is a custom `Editor` class-file generator based-on Unity Tags, Layer and SortingLayers built-it engine enums. Generated classes provide at runtime fast access to actual Tag/Layers/SortingLayers constants.

# How to use:
- For generate go to `Editor > Project Settings > Tags ans Layer > Generator`.
- When you change something in `Editor (Tags/Layers)` don't forget to update classes. Generator runs manually without editor's hooks for better editor performance.

## Settings
Create settings file.
- CreateAsset menu: `Scriptable/Tlgs/Settings File`.
- Settings file contains:
  - gen. class name.
  - gen. class path.
  - namespace.

##Generated constants:
#### Tags:
class contains: `string` tag names.
#### Layers:
class contains: `string` layer name, `int` mask, `int` number.
#### SortingLayers:
class contains: `string` layer names and `int` layer id.