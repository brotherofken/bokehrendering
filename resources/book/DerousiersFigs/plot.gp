set terminal postscript enhanced color
set key left top
set style line 1 linetype 1 lc rgb "#00BFFF" lw 3
set style line 2 linetype 3 lc rgb "red" lw 3
set style line 3 linetype 3 lc rgb "blue" lw 3
set style line 4 linetype 3 lc rgb "green" lw 3
set style line 5 linetype 3 lc rgb "orange" lw 3
set style line 6 linetype 3 lc rgb "#FEB00F" lw 3
set xlabel "Number of bokehs" font "Helvetica,18"
set ylabel "Time (ms)" font "Helvetica,18"
set xrange[2000:11000]

set output 'performances.eps'
plot \
"performances.dat" using 1:2 with lines ls 1 title "Reset", \
"performances.dat" using 1:3 with lines ls 2 title "Linear Depth / CoC", \
"performances.dat" using 1:4 with lines ls 3 title "Bokeh detection", \
"performances.dat" using 1:5 with lines ls 4 title "Blur", \
"performances.dat" using 1:6 with lines ls 5 title "Synchronization", \
"performances.dat" using 1:7 with lines ls 6 title "Bokeh rendering"

