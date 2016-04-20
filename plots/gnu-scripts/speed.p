# Gnuplot script file for plotting data using the file 'data-raw.txt'
# Plotting speed

# set term png size 1200,600
# set output 'speed-plot.png'

set multiplot layout 1,3 title "speed of 34 individuals | pearson ~ 0.094, spearman ~ 0.189" font ',14'
set key font ',11'

# set size 0.33, 0.95
set title "crossed individuals"
set xlabel "# Individual"                      font ',13'
set ylabel "Speed"                             font ',13'
set xrange [-1:35]
set yrange [0.5:2.75]
plot "../data-crossed.txt" using 16 title 'crossed-speed' ,\
     2.304                       title 'crossed-mean'  ,\
     1.625                       title 'mean'

# set size 0.33, 0.95
# set origin 0.33, 0
set title "non-crossed individuals"
set xlabel "# Individual"                      font ',13'
set ylabel "Speed"                             font ',13'
set xrange [-1:35]
set yrange [0.5:2.75]
plot "../data-non.txt"     using 16 title 'non-speed' , \
     1.900                       title 'non-mean'  , \
     1.625                       title 'mean'

# set size 0.33, 0.95
# set origin 0.66, 0
set title "individuals-pairs"
set xlabel "crossed-speed"                  font ',13'
set ylabel "non-speed"                      font ',13'
set xrange [0.5:2.75]
set yrange [0.5:2.75]
plot "../data-summed.txt"   using 16:34 title 'cross <-> non'

unset multiplot