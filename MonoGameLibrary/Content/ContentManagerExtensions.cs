using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.Content;

public static class ContentManagerExtensions
{
    public static WatchedAsset<T> Watch<T>(this ContentManager manager, string assetName)
    {
        var asset = manager.Load<T>(assetName);
        return new WatchedAsset<T>
        {
            AssetName = assetName,
            Asset = asset,
            UpdatedAt = DateTimeOffset.Now,
            Owner = manager,
        };
    }

    /// <summary>  
    /// Load an Effect into the <see cref="Material"/> wrapper class  
    /// </summary>  
    /// <param name="manager"></param>  
    /// <param name="assetName"></param>  
    /// <returns></returns>  
    public static Material WatchMaterial(this ContentManager manager, string assetName)
    {
        return new Material(manager.Watch<Effect>(assetName));
    }


    public static bool TryRefresh<T>(this ContentManager manager, WatchedAsset<T> watchedAsset, out T oldAsset)
    {
        oldAsset = default;

        // ensure the ContentManager is the same one that loaded the asset
        if (manager != watchedAsset.Owner)
        {
            throw new ArgumentException($"Used the wrong ContentManager to refresh {watchedAsset.AssetName}");
        }

        // get the same path that the ContentManager would use to load the asset
        var path = Path.Combine(manager.RootDirectory, watchedAsset.AssetName) + ".xnb";

        // ask the operating system when the file was last written.
        var lastWriteTime = File.GetLastWriteTime(path);

        //  when the file's write time is less recent than the asset's latest read time, 
        //  then the asset does not need to be reloaded.
        if (lastWriteTime <= watchedAsset.UpdatedAt)
        {
            return false;
        }

        // wait for the file to not be locked.
        if (IsFileLocked(path)) return false;

        // clear the old asset to avoid leaking
        manager.UnloadAsset(watchedAsset.AssetName);

        // return the old asset
        oldAsset = watchedAsset.Asset;

        // load the new asset and update the latest read time
        watchedAsset.Asset = manager.Load<T>(watchedAsset.AssetName);
        watchedAsset.UpdatedAt = lastWriteTime;

        return true;
    }


    private static bool IsFileLocked(string path)
    {
        try
        {
            using FileStream _ = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            // File is not locked
            return false;
        }
        catch (IOException)
        {
            // File is locked or inaccessible
            return true;
        }
    }

    [Conditional("DEBUG")]
    public static void StartContentWatcherTask()
    {
        var args = Environment.GetCommandLineArgs();
        foreach (var arg in args)
        {
            // if the application was started with the --no-reload option, then do not start the watcher.
            if (arg == "--no-reload") return;
        }

        // identify the project directory
        var projectFile = Assembly.GetEntryAssembly().GetName().Name + ".csproj";
        var current = Directory.GetCurrentDirectory();
        string projectDirectory = null;

        while (current != null && projectDirectory == null)
        {
            if (File.Exists(Path.Combine(current, projectFile)))
            {
                // the valid project csproj exists in the directory
                projectDirectory = current;
            }
            else
            {
                // try looking in the parent directory.
                //  When there is no parent directory, the variable becomes 'null'
                current = Path.GetDirectoryName(current);
            }
        }

        // if no valid project was identified, then it is impossible to start the watcher
        if (string.IsNullOrEmpty(projectDirectory)) return;

        // start the watcher process
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "build -t:WatchContent --tl:off",
            WorkingDirectory = projectDirectory,
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            CreateNoWindow = false
        });

        // when this program exits, make sure to emit a kill signal to the watcher process
        AppDomain.CurrentDomain.ProcessExit += (_, __) =>
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                }
            }
            catch
            {
                /* ignore */
            }
        };
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                }
            }
            catch
            {
                /* ignore */
            }
        };
    }

}