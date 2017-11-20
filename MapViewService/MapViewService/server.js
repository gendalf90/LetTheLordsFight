﻿const jsonServer = require('json-server');
const server = jsonServer.create();
const middlewares = jsonServer.defaults();

server.use(middlewares);

//server.use(jsonServer.rewriter({
//    '/api/echo': '/echo'
//}));

server.get('/api/v1/map', (req, res) => {
    res.json({
        segments: {
            width: 10,
            height: 10
        }
    });
});

server.listen(25000, () => {
    console.log('Server is running')
});