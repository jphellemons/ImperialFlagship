# Imperial Flagship

I started this project to find the bricks for this amazing set. 
It was based on reading an CSV file with all the pieces, so you can use this project for any other set and not just the flagship. I have used the rebrickable api now, so that it is no longer required to have an CSV file or xml with the pieces listed.
I just did not feel like making a GUI for it, so it is a command line tool. Perhaps there will be an WPF gui or windows phone app.
I would like some pull-requests for it. :-)
It uses the Bricklink website to scrape the info from, but I was planning to use the upcoming REST buyer api. At the moment there is only a store api.

## Future planning ##

- Add custom logic to limit to stores of a specific country
- Add logic which also use the shipping to calculate it
- Add logic to limit the number of shops used. It now just give a percentage of availability

## Changelog ##

- added the ability to start the tool with a set id. for instance: "imperialflagship 10210-1" or "imperialflagship 70810-1" for metalbeards seacow. This is ofcourse not limited to boat sets only and can be used with all lego sets
- the rebrickable api is now used to retrieve a part list of the set. This site is awesome.

![Lego Imperialflagship 10210-1](http://www.1000steine.com/brickset/large/10210-1.jpg)

![Lego The seacow 70810-1](http://www.1000steine.com/brickset/large/70810-1.jpg)