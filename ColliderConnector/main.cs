using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.MemoryMappedFiles;
using Harmony;
using UnhollowerBaseLib.Runtime;
using UnhollowerRuntimeLib.XrefScans;

using VRC;
using VRC.Core;

using SphereColliderList = Il2CppSystem.Collections.Generic.List<UnityEngine.SphereCollider>;
using BoxColliderList = Il2CppSystem.Collections.Generic.List<UnityEngine.BoxCollider>;
using CapsuleColliderList = Il2CppSystem.Collections.Generic.List<UnityEngine.CapsuleCollider>;
using Harmony.ILCopying;
using Il2CppSystem.IO;
using UnhollowerBaseLib;
using Il2CppSystem.Runtime.InteropServices;

namespace ColliderConnector
{

    public static class BuildInfo
    {
        public const string Name = "ColliderCon"; // Name of the Plugin.  (MUST BE SET)
        public const string Description = "Clider Connector API"; // Description for the Plugin.  (Set as null if none)
        public const string Author = "XAYRGA"; // Author of the Plugin.  (Set as null if none)
        public const string Company = null; // Company that made the Plugin.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Plugin.  (MUST BE SET)
        public const string DownloadLink = "http://www.xayr.ga/project/collidercon"; // Download Link for the Plugin.  (Set as null if none)
    }

    public unsafe class ColliderConnector : MelonMod
    {

        private static readonly SphereColliderList SphereColliders = new SphereColliderList();
        private static readonly BoxColliderList BoxColliders = new BoxColliderList();
        private static readonly CapsuleColliderList CapsuleColliders = new CapsuleColliderList();
        private static readonly MemoryMappedFile memMap = MemoryMappedFile.CreateNew("ColliderconVR", 10000);
        private static int* IPCArena; 

        private static void GetAllColliders<T>(Il2CppSystem.Collections.Generic.List<T> colliderList)
        {
         
            colliderList.Clear();
            var sceneCount = SceneManager.sceneCount;
            for (var i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var rootGameObjects = scene.GetRootGameObjects();
                foreach (var gameObject in rootGameObjects)
                {
                    var colliders =
                        gameObject.GetComponentsInChildren<T>();
                    foreach (var collider in colliders)
                    {
                        colliderList.Add(collider);
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            
            GetAllColliders(SphereColliders);
            IPCArena[1] = SphereColliders.Count; // store map collider count in first 4 bytes
            byte* IPCB = (byte*)IPCArena;
            var mOffsetPos = 0;
            foreach (SphereCollider sp in SphereColliders)
            {
                var xb = BitConverter.GetBytes(sp.transform.position.x);
                var yb = BitConverter.GetBytes(sp.transform.position.y);
                var zb = BitConverter.GetBytes(sp.transform.position.z);
                var rad = BitConverter.GetBytes(sp.radius);
                var zb1 = Encoding.ASCII.GetBytes(sp.gameObject.name);
                    ///Console.WriteLine($"{sp.gameObject.transform.position.x},{sp.gameObject.transform.position.y},{sp.gameObject.transform.position.z}   ");
                for (int i=0; i < xb.Length;i++)
                {
                    IPCB[9 + mOffsetPos] = xb[i];
                    mOffsetPos++;
                }

                for (int i = 0; i < yb.Length; i++)
                {
                    IPCB[9 + mOffsetPos] = yb[i];
                    mOffsetPos++;
                }

                for (int i = 0; i < zb.Length; i++)
                {
                    IPCB[9 + mOffsetPos] = zb[i];
                    mOffsetPos++;
                }

                IPCB[9 + mOffsetPos] = (byte)zb1.Length;
                mOffsetPos++;
                for (int i=0; i < zb1.Length; i++)
                {
                    IPCB[9 + mOffsetPos] = zb1[i];
                    mOffsetPos++;
                }
                for (int i = 0; i < rad.Length; i++)
                {
                    IPCB[9 + mOffsetPos] = rad[i];
                    mOffsetPos++;
                }
            }
        }


        public unsafe override void OnApplicationStart() // Runs after Game Initialization.
        {
            MelonLogger.Log("OnApplicationStart -- COLLIDERCON!!!");
            var viewAccessor = memMap.CreateViewAccessor();
            byte* poke = null;
            viewAccessor.SafeMemoryMappedViewHandle.AcquirePointer(ref poke);
            int* poked = (int*)poke;
            IPCArena = poked;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("!!!!!!! IPCArena opened at {0:X} !!!!!!!", (long)poked );
            Console.ForegroundColor = ConsoleColor.White;
        }


        public override void OnApplicationQuit() // Runs when the Game is told to Close.
        {
            MelonLogger.Log("OnApplicationQuit");
        }

        public override void OnModSettingsApplied() // Runs when Mod Preferences get saved to UserData/modprefs.ini.
        {
            MelonLogger.Log("OnModSettingsApplied");
        }
    }


}
