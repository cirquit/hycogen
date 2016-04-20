{-# LANGUAGE MultiParamTypeClasses, Rank2Types, RecordWildCards, BangPatterns #-}

module SimpleStatistics where

import Prelude hiding (null)
import Data.Foldable  (Foldable(), null, toList)
import Data.List      (foldl', sort, lookup, genericLength)
import Data.Maybe     (fromJust)

data Stat b = Stat
    { mean     :: Floating b => b
    , var      :: Floating b => b
    , std      :: Floating b => b
    , len      :: Num b      => b
    , meanDiff :: Floating b => [b]
    , values   :: Floating b => [b]
    } 


-- | 
toStat :: (Foldable t, Real a, Floating b) => t a -> Stat b
toStat xs
        | null xs   = error "simple-stats.hs: toStat - argument is empty"
        | otherwise = Stat mean var std len meanDiff xss
    where
        xss = map (fromRational . toRational) $ toList xs

        (len, mean, psum, meanDiff) =
            (\(l, s, ps, md) -> (l, s / l, ps, reverse md)) . foldl' go (0,0,0,[]) $ xss

        go (l, s, ps, md) x =
            let xm = x - mean
            in ( l  + 1         -- length
               , s  + x         -- sum
               , ps + xm**2     -- squared diff-mean sum
               , xm : md        -- mean difference as list
               )
        var               = psum / len  -- variance
        std               = sqrt var    -- standart deviation


-- | Creating a ranked representation
--
toRank :: (Floating a, Floating b, Ord a) => Stat a -> Stat b
toRank sX = toStat rval
    where 
        rval = ranked (values sX)

        -- | very inefficient - O(n^2 + n*logn + n)
        ranked :: (Ord a, Num a, Num b, Enum b) => [a] -> [b]
        ranked xs = map (\x -> fromJust $ lookup x values) xs
            where values = zip (sort xs) [1..]


-- | Covariance
--
cov :: (Show a, Eq a, Floating a) => Stat a -> Stat a -> a
cov sX sY
  | len sX /= len sY = error $ unlines
      ["simple-stats.hs: cov - can't create a covariance computation because the length of the values don't match"
      ,"len x:" ++ show (len sX)
      ,"len y:" ++ show (len sY)
      ]
  | otherwise        = (/ len sX) . sum $ zipWith (*) (meanDiff sX) (meanDiff sY)


-- | Pearson product movement correleation
--
--   Normalized covariance, -1 ^= total negative correlation, +1 ^= total positive correletaion, 0 ^= no correleation
--
--    |         *          | *                  |       *   *    
--    |       *            |   *                |  *   *         
--    |     *              |     *              |   *    *   *   
--    |   *                |       *            |     *          
--    | *                  |         *          | *  *    *    * 
--   _|_______________    _|_______________    _|_______________
--    |    + 1             |    - 1             |       0        
--
--
pcorr :: (Eq a, Floating a, Show a) => Stat a -> Stat a -> a
pcorr sX sY = cov sX sY / (std sX * std sY)


-- | Spearmans rank correlation coefficient
--
--   This coefficient isn't that much affected by outliers
scorr :: (Eq a, Floating a, Show a, Ord a) => Stat a -> Stat a -> a
scorr sX sY = pcorr (toRank sX) (toRank sY)

createStats ::
    (Floating b, Floating b1, Real a, Real a1) =>
    [a1] -> (a1 -> a) -> (Stat b1, Stat b)
createStats xs f =
    let !xS = toStat xs
        !yS = toStat $ map f xs
    in (xS, yS)


--simple-stats.hs:97:22:
--    My brain just exploded
--    I can't handle pattern bindings for existential or GADT data constructors.
--    Instead, use a case-expression, or do-notation, to unpack the constructor.
--    In the pattern: Stat mean std len values
--    In the pattern: Right stat@(Stat mean std len values)
--    In a pattern binding:
--      (Right stat@(Stat mean std len values)) = toStat xs
