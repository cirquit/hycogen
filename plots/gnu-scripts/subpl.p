# Gnuplot script file for plotting data using the file 'data-raw.txt'
# Plotting subPathLength

# set term png size 1200,600
# set output 'speed-plot.png'

set multiplot layout 1,3 title "subPathLength of 34 individuals | pearson ~ -0.182, spearman ~ -0.263" font ',14'
set key font ',11'

# set size 0.33, 0.95
set title "crossed individuals"
set xlabel "# Individual"                      font ',13'
set ylabel "SubPathLength"                     font ',13'
set xrange [-1:35]
set yrange [0.5:4.0]
plot "../data-crossed.txt" using 8 title 'crossed-subPL' ,\
     2.863                         title 'crossed-mean'  ,\
     2.250                         title 'mean'

# set size 0.33, 0.95
# set origin 0.33, 0
set title "non-crossed individuals"
set xlabel "# Individual"                      font ',13'
set ylabel "SubPathLength"                     font ',13'
set xrange [-1:35]
set yrange [0.5:4.0]
plot "../data-non.txt"     using 8 title 'non-speed' , \
     2.406                       title 'non-mean'  , \
     2.250                       title 'mean'

# set size 0.33, 0.95
# set origin 0.66, 0
set title "individuals-pairs"
set xlabel "crossed-subPL"                  font ',13'
set ylabel "non-subPL"                      font ',13'
set xrange [0.5:4.0]
set yrange [0.5:4.0]
plot "../data-summed.txt"   using 8:26 title 'cross <-> non'

unset multiplot