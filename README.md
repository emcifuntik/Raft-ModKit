# Raft-ModKit
![Raft Poster](/raft.png)
## How to start playing with mods
* Download actual version of ModKit
* Unpack .zip archive anywhere you want
* Put mods that you need inside plugins folder
* Start **Launcher.exe** and enjoy

## How to Build

## How to start modding
* Create new C# .Net Framework Library project named _SomethingHere_Script_. **Important:** Your assembly filename must ends with Script.dll
* Add references to 
  * UnityEngine
  * Assembly-CSharp.dll
* Extend you Script class with **UnityEngine.MonoBehaviour**
* Now you subscribed to Unity scene messages. [Look here](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)


## References
* [Unity DLL Injector](https://github.com/aw-3/Unity-Injector)
* [Mono inject tutorial](https://www.unknowncheats.me/forum/rust/114627-loader-titanium-alternative.html)
* [Minhook used to hook Win32 functions](https://github.com/TsudaKageyu/minhook)
