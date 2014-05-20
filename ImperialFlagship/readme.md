# Imperial Flagship

I started this project to find the bricks for this amazing set. 
It is based on reading an CSV file with all the pieces, so you can use this project for any other set.
I just did not feel like making a GUI for it, so it is a command line tool with the csv file hardcoded in it. So you have to build it from source.
I would like some pull-requests for it.
It uses the Bricklink website to scrape the info from, but I was planning to use the upcoming REST buyer api.

## Future planning ##

- Add custom logic to limit to stores of a specific country
- Add logic which also use the shipping to calculate it
- Add logic to limit the number of shops used. It now just give a percentage of availability

## Changelog ##

- added the ability to start the tool with a set id. for instance: "imperialflagship 10210-1" or "imperialflagship 70810" for metalbeards seacow. This is ofcourse not limited to boat sets only and can be used with all lego sets
- the rebrickable api is now used to retrieve a part list of the set. This site is awesome. I will not include my api key (perhaps in binary build) in the source code.


![Alt text](http://www.1000steine.com/brickset/large/10210-1.jpg)