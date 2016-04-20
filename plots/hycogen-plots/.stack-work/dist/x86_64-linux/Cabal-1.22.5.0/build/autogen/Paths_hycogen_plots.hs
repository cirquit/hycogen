module Paths_hycogen_plots (
    version,
    getBinDir, getLibDir, getDataDir, getLibexecDir,
    getDataFileName, getSysconfDir
  ) where

import qualified Control.Exception as Exception
import Data.Version (Version(..))
import System.Environment (getEnv)
import Prelude

catchIO :: IO a -> (Exception.IOException -> IO a) -> IO a
catchIO = Exception.catch

version :: Version
version = Version [0,1,0,0] []
bindir, libdir, datadir, libexecdir, sysconfdir :: FilePath

bindir     = "/home/rewrite/Documents/Uni/Semester6/Bachelorarbeit/hycogen/plots/hycogen-plots/.stack-work/install/x86_64-linux/lts-5.13/7.10.3/bin"
libdir     = "/home/rewrite/Documents/Uni/Semester6/Bachelorarbeit/hycogen/plots/hycogen-plots/.stack-work/install/x86_64-linux/lts-5.13/7.10.3/lib/x86_64-linux-ghc-7.10.3/hycogen-plots-0.1.0.0-25x0iaQiCk9Lw4BqRmRuNN"
datadir    = "/home/rewrite/Documents/Uni/Semester6/Bachelorarbeit/hycogen/plots/hycogen-plots/.stack-work/install/x86_64-linux/lts-5.13/7.10.3/share/x86_64-linux-ghc-7.10.3/hycogen-plots-0.1.0.0"
libexecdir = "/home/rewrite/Documents/Uni/Semester6/Bachelorarbeit/hycogen/plots/hycogen-plots/.stack-work/install/x86_64-linux/lts-5.13/7.10.3/libexec"
sysconfdir = "/home/rewrite/Documents/Uni/Semester6/Bachelorarbeit/hycogen/plots/hycogen-plots/.stack-work/install/x86_64-linux/lts-5.13/7.10.3/etc"

getBinDir, getLibDir, getDataDir, getLibexecDir, getSysconfDir :: IO FilePath
getBinDir = catchIO (getEnv "hycogen_plots_bindir") (\_ -> return bindir)
getLibDir = catchIO (getEnv "hycogen_plots_libdir") (\_ -> return libdir)
getDataDir = catchIO (getEnv "hycogen_plots_datadir") (\_ -> return datadir)
getLibexecDir = catchIO (getEnv "hycogen_plots_libexecdir") (\_ -> return libexecdir)
getSysconfDir = catchIO (getEnv "hycogen_plots_sysconfdir") (\_ -> return sysconfdir)

getDataFileName :: FilePath -> IO FilePath
getDataFileName name = do
  dir <- getDataDir
  return (dir ++ "/" ++ name)
