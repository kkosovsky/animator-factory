using System;

namespace AnimatorFactory
{
    public static class Helpers
    {
        /// <summary>
        /// Checks if the asset path belongs to a Unity package.
        /// </summary>
        /// <param name="assetPath">The asset path to check</param>
        /// <returns>True if the asset is from a package, false otherwise</returns>
        ///
        const string packages = "Packages/";
        const string libraryPackageCache = "Library/PackageCache/";

        public static bool IsFromUnityPackage(string assetPath)
        {
            if (string.IsNullOrEmpty(value: assetPath))
                return false;

            return assetPath.StartsWith(value: packages, comparisonType: StringComparison.OrdinalIgnoreCase)
                || assetPath.StartsWith(
                    value: libraryPackageCache,
                    comparisonType: StringComparison.OrdinalIgnoreCase
                );
        }
    }
}
