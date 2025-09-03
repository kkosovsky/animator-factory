using System;
using System.IO;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariant
    {
        public string name => gameObject.name;
        public string fullSpritesSourcePath;
        public string fullClipsDestinationPath;

        public readonly GameObject gameObject;
        public string spriteSourcesDirPath;
        public string generatedClipsPath;
        public Guid id;
        

        public PrefabVariant(GameObject gameObject)
        {
            this.gameObject = gameObject;
            id = Guid.NewGuid();
            generatedClipsPath = AnimatorFactoryWindow.Configuration.GeneratedClipsPath;
            fullClipsDestinationPath = $"{generatedClipsPath}{gameObject.name}{Path.DirectorySeparatorChar}";
            
            spriteSourcesDirPath = AnimatorFactoryWindow.Configuration.DefaultSourceSpritePath;
            fullSpritesSourcePath = $"{spriteSourcesDirPath}{gameObject.name}{Path.DirectorySeparatorChar}";
        }
    }
}
