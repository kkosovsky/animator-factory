using System;
using System.IO;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariant
    {
        public string name => gameObject.name;

        public readonly GameObject gameObject;
        public string spriteSourcesDirPath;
        public string generatedClipsPath;
        public Guid id;

        public PrefabVariant(GameObject gameObject)
        {
            string assetsPath = $"Assets{Path.DirectorySeparatorChar}";
            this.gameObject = gameObject;
            id = Guid.NewGuid();
            generatedClipsPath = assetsPath;
            spriteSourcesDirPath = assetsPath;
        }
    }
}
