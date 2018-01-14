const jsonServer = require('json-server');
const server = jsonServer.create();
const middlewares = jsonServer.defaults();

server.use(middlewares);

server.post('/api/v1/users', (req, res) => {
    res.sendStatus(200);

    //res.status(400).json({
    //    login: ['error1', 'error2'],
    //    password: ['error3']
    //});
});

server.get('/api/v1/users/current/token', (req, res) => {
    res.send('test_123');

    //res.sendStatus(401);
});

server.listen(25200, () => {
    console.log('Server is running')
});