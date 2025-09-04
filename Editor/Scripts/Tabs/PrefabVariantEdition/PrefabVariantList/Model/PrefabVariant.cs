using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AnimatorFactory.PrefabVariants
{
    public class PrefabVariant
    {
        public string name => gameObject.name;

        public readonly string fullSpritesSourcePath;
        public readonly string fullClipsDestinationPath;
        public readonly string fallbackSpritePath;

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
            string baseName = name.Split(separator: '_').First();
            fullSpritesSourcePath = $"{spriteSourcesDirPath}{baseName}{Path.DirectorySeparatorChar}";

            fallbackSpritePath =
                $"{AnimatorFactoryWindow.Configuration.FallbackSpritesPath}{baseName}.png";
        }
    }
}
