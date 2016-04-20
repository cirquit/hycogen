# Gnuplot script file for plotting data using the file 'data-raw.txt'
# Plotting speed

# set term png size 1200,600
# set output 'speed-plot.png'

set multiplot layout 1,3 title "Targetcollision of 34 individuals | pearson ~ -0.181, spearman ~ -5.800" font ',14'
set key font ',11'

# set size 0.33, 0.95
set title "crossed individuals"
set xlabel "# Individual"                      font ',13'
set ylabel "TC"                             font ',13'
set xrange [-1:35]
set yrange [-100:100]
plot "../data-crossed.txt" using 5 title 'crossed-TC' ,\
     3.900                  title 'crossed-mean'  ,\
     0                       title 'mean'

# set size 0.33, 0.95
# set origin 0.33, 0
set title "non-crossed individuals"
set xlabel "# Individual"                      font ',13'
set ylabel "TC"                             font ',13'
set xrange [-1:35]
set yrange [-100:100]
plot "../data-non.txt"     using 5 title 'non-RC' , \
     36.853                      title 'non-mean'  , \
     0                           title 'mean'

# set size 0.33, 0.95
# set origin 0.66, 0
set title "individuals-pairs"
set xlabel "crossed-TC"                  font ',13'
set ylabel "non-TC"                      font ',13'
set xrange [-100:100]
set yrange [-100:100]
plot "../data-summed.txt"   using 5:23 title 'cross <-> non'

unset multiplot