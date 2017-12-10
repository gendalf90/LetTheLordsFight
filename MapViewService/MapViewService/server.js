'use strict'

const jsonServer = require('json-server');
const server = jsonServer.create();
const middlewares = jsonServer.defaults();

server.use(middlewares);

//server.use(jsonServer.rewriter({
//    '/api/echo': '/echo'
//}));

server.get('/api/v1/map', (req, res) => {
    res.json({
        segments: {
            width: 5,
            height: 5
        }
    });
});

server.get('/api/v1/map/segments/square5x5/i/:i/j/:j', (req, res) => {

    const size = 10;
    let segments = [];

    for (let i = 0; i < 5; i++) {
        for (let j = 0; j < 5; j++) {
            segments.push({
                i,
                j,
                leftx: j * size,
                rightx: (j + 1) * size,
                upy: i * size,
                downy: (i + 1) * size,
                type: 'Grass'
            });
        }
    }

    let toForest = segments.find(segment => segment.i == 1 && segment.j == 1);
    toForest.type = 'Forest';

    let objects = [{
        id: 'other@other.ru',
        location: {
            x: 44,
            y: 27
        }
    }];

    res.json({
        segments,
        objects
    })
});

server.get('/api/v1/map/objects/:id', (req, res) => {
    res.json({
        id: 'test@test.ru',
        location: {
            x: 25,
            y: 25
        },
        visible: true,
        segment: {
            i: 2,
            j: 2
        }
    });
});

server.patch('/api/v1/map/objects/:id', (req, res) => {
    res.sendStatus(200);
});

server.listen(25000, () => {
    console.log('Server is running')
});