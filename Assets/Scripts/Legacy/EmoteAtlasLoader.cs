// using UnityEngine;
// using UnityEngine.U2D;
//
// public static class EmoteAtlasLoader
// {
//     // 필요하면 캐릭터별로 Dictionary로 확장 가능
//     private static SpriteAtlas _mutsuki;
//
//     public static Sprite GetMutsukiEmote(string spriteName)
//     {
//         // 1) Atlas lazy-load + cache
//         if (_mutsuki == null)
//         {
//             _mutsuki = Resources.Load<SpriteAtlas>("Mutsuki/Atlas_Emote_Mutsuki");
//             if (_mutsuki == null)
//             {
//                 Debug.LogError("[EmoteAtlasLoader] Failed to load SpriteAtlas: Resources/Mutsuki/Atlas_Emote_Mutsuki");
//                 return null;
//             }
//         }
//
//         // 2) Fetch sprite by name (Sprite 이름 = Mutsuki_Emote_Baffled 같은 그 이름)
//         Sprite s = _mutsuki.GetSprite(spriteName);
//         if (s == null)
//             Debug.LogWarning($"[EmoteAtlasLoader] Sprite not found in atlas: {spriteName}");
//
//         return s;
//     }
// }