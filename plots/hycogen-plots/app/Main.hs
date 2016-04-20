module Main where

import Data.List (foldl')
import Data.List.Split (splitOn)
import System.IO
import SimpleStatistics

getData :: IO [[Double]]
getData = do
    input <- readFile "../data-raw.txt"
    let input' = foldl' (\xs x -> if null x then xs else x:xs) [] (lines input)
        parsed = map (map (read . drop 1 . dropWhile (/= ':')) . splitOn ",") input'
    return parsed

getCrossedData :: IO [[Double]]
getCrossedData = do
    input <- getData
    let zipd = zip [1..] input
    return [str | (x, str) <- zipd, x `mod` 2 == 0]

getNonData :: IO [[Double]]
getNonData = do
    input <- getData
    let zipd = zip [1..] input
    return [str | (x, str) <- zipd, x `mod` 2 == 1]

getInfoAt i d = map (!! i) d

main :: IO ()
main = return ()



--    public static Tuple<int,int>      collisionBounds       = new Tuple<int, int>(-100, 100);
--    public static Tuple<float,float>  greekBounds           = new Tuple<float, float>(0.0f, 1.0f);
--    public static Tuple<int,int>      popSizeBounds         = new Tuple<int, int>(2, 10);
--    public static Tuple<int,int>      subPathCountBounds    = new Tuple<int, int>(2, 5);
--    public static Tuple<float,float>  subPathLengthBounds   = new Tuple<float, float>(0.5f, 4.0f);
--    public static Tuple<int,int>      generationCountBounds = new Tuple<int, int>(1, 10);
--    public static Tuple<int,int>      modeBounds            = new Tuple<int, int>(1, 2);
--    public static Tuple<float,float>  maxDeviationBounds    = new Tuple<float, float>(0.5f, 5.0f);
--    public static Tuple<float,float>  speedBounds           = new Tuple<float, float>(0.5f, 2.75f);
