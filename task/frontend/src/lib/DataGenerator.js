import { DateHelper, RandomGenerator } from '@bryntum/grid';

export default class DataGenerator {
    //region Random

    static reset() {
        this.rnd.reset();
        this.rndTime.reset();
        this.rndRating.reset();
    }

    //endregion

    //region Generate data

    static * generate(count, randomHeight = false, initialId = 1) {
        const me         = this,
            rnd        = me.rnd,
            rndTime    = me.rndTime,
            rndRating  = me.rndRating,
            firstNames = me.firstNames,
            surNames   = me.surNames,
            teams      = me.teams,
            foods      = me.foods,
            colors     = me.colors,
            cities     = me.cities;

        for (let i = 0; i < count; i++) {
            const firstName = rnd.fromArray(firstNames),
                surName   = rnd.fromArray(surNames),
                name      = `${firstName} ${String.fromCharCode(
                    65 + (i % 25)
                )} ${surName}`,
                startDay  = rnd.nextRandom(60) + 1,
                start     = new Date(2019, 0, startDay),
                finish    = new Date(2019, 0, startDay + rnd.nextRandom(30) + 2), //DateHelper.add(start, rnd.nextRandom(30) + 2, 'days'),
                row       = {
                    id    : i + initialId,
                    title : 'Row ' + i,
                    name,
                    firstName,
                    surName,
                    city  : rnd.fromArray(cities),
                    team  : rnd.fromArray(cities) + ' ' + rnd.fromArray(teams),
                    age   : 10 + rnd.nextRandom(80),
                    food  : rnd.fromArray(foods),
                    color : rnd.fromArray(colors),
                    score : rnd.nextRandom(100) * 10,
                    rank  : rnd.nextRandom(100) + 1,
                    start,
                    finish,
                    time  : DateHelper.getTime(
                        rndTime.nextRandom(24),
                        rndTime.nextRandom(12) * 5
                    ),
                    percent   : rnd.nextRandom(100),
                    done      : rnd.nextRandom(100) < 50,
                    rating    : rndRating.nextRandom(5),
                    relatedTo : Math.min(
                        count - 1,
                        i + initialId + rnd.nextRandom(10)
                    )
                };

            if (randomHeight) {
                row.rowHeight =
                    rnd.nextRandom(randomHeight === true ? 20 : randomHeight) *
                    5 +
                    20;
            }

            yield row;
        }
    }

    // Param reset defaults to true to ensure we get the same dataset on consecutive calls. Without it, code editor
    // reloading modules yields different data each time
    static generateData(
        count,
        randomHeight = false,
        initialId    = 1,
        reset        = true
    ) {
        if (reset) this.reset();
        const rows      = [],
            number    = DataGenerator.overrideRowCount
                ? DataGenerator.overrideRowCount
                : count,
            generator = this.generate(number, randomHeight, initialId);

        for (let i = 0; i < number; i++) {
            rows.push(generator.next().value);
        }

        return rows;
    }

    //endregion

    //region generate tree data

    // @todo Tree generation is hardwired for the purposes of this
    // example only. Needs refactoring should it be universal
    static generateTreeData(count = 500, childCount = 100) {
        const rows = [];

        for (let i = 0; i < count; i++) {
            const row = this.generate(1, false, i + 1).next().value;
            Object.assign(row, {
                name   : row.city,
                food   : '',
                team   : '',
                color  : '',
                rating : ''
            });
            this.generateChildren(row, childCount);
            row.city = '';
            rows.push(row);
        }
        return rows;
    }

    static generateChildren(row, count) {
        const children  = [],
            generator = this.generate(count, false, row.id * 100000);
        for (let i = 0; i < count; i++) {
            const child = generator.next().value;
            Object.assign(child, {
                parentId : row.id,
                city     : row.city
            });
            children.push(child);
        }
        row.children = children;
    }

    /* #endregion */
}

Object.assign(DataGenerator, {
    rnd       : new RandomGenerator(),
    rndTime   : new RandomGenerator(),
    rndRating : new RandomGenerator(),
    cities    : [
        'Stockholm',
        'Barcelona',
        'Paris',
        'Dubai',
        'New York',
        'San Francisco',
        'Washington',
        'Moscow'
    ],
    firstNames : [
        'Mike',
        'Linda',
        'Don',
        'Karen',
        'Doug',
        'Jenny',
        'Daniel',
        'Melissa',
        'John',
        'Jane',
        'Theo',
        'Lisa',
        'Adam',
        'Mary',
        'Barbara',
        'James',
        'David'
    ],
    surNames : [
        'McGregor',
        'Ewans',
        'Scott',
        'Smith',
        'Johnson',
        'Adams',
        'Williams',
        'Brown',
        'Jones',
        'Miller',
        'Davis',
        'More',
        'Wilson',
        'Taylor',
        'Anderson',
        'Thomas',
        'Jackson'
    ],
    teams : [
        'Lions',
        'Eagles',
        'Tigers',
        'Horses',
        'Dogs',
        'Cats',
        'Panthers',
        'Rats',
        'Ducks',
        'Cougars',
        'Hens',
        'Roosters'
    ],
    foods : [
        'Pancake',
        'Burger',
        'Fish n chips',
        'Carbonara',
        'Taco',
        'Salad',
        'Bolognese',
        'Mac n cheese',
        'Waffles'
    ],
    colors : [
        'Blue',
        'Green',
        'Red',
        'Yellow',
        'Pink',
        'Purple',
        'Orange',
        'Teal',
        'Black'
    ]
});
