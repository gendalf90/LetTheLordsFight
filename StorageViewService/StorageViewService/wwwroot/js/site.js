var app = angular.module('storageApp', []);

app.controller("myStorageController", function ($scope, storageService) {
    
    $scope.items = [];

    $scope.selected = [];
    
    $scope.select = function (itemId) {
        
        function itemIndexEqual(value) {
            return value.id === itemId;
        }

        var findIndexInItems = $scope.items.findIndex(itemIndexEqual);
        var findIndexInSelected = $scope.selected.findIndex(itemIndexEqual);

        if (findIndexInSelected !== -1) {
            $scope.selected.splice(findIndexInSelected, 1);
        } else if (findIndexInItems !== -1) {
            var newItemToSelected = $scope.items[findIndexInItems];
            $scope.selected.push(newItemToSelected);
        }
    }

    $scope.isSelected = function (itemId) {
        return $scope.selected.some(function (value) {
            return value.id === itemId;
        });
    }


    $scope.load = function (storageId) {
        storageService.getItems(storageId).then(function (value) { $scope.items = value });
    }
});

app.controller("storageController", function ($scope, storageService) {



    $scope.storages = [];

    $scope.items = [];

    $scope.isOwner = false;

    $scope.init = function () {
        $scope.storages.push({ id: 1 });
    }

    $scope.loadInteractableFrom = function (storageId) {
        storageService.getStorage(storageId).then(function (value) { $scope.storages = value.data.interactions; });
    }

    $scope.load = function (storageId) {
        storageService.getItems(storageId).then(function (value) { $scope.items = value });
    }
});

app.factory("storageService", function ($http, $q) {
    return {
        getItems : function(storageId) {
            let d = $q.defer();
            //api/v1/storage/{storageId}/items/

            let request = {
                method: "GET",
                url: "http://192.168.1.8:8080/api/v1/storage/" + storageId + "/items/",
                //url: "/view/storage/test",
                headers: {
                    Authorization: "Bearer " + localStorage["token"]
                }
            }

            $http(request).then(function (response) {
                d.resolve(response.data);
            }, function (response) {
                d.reject(response.status);
            });

            return d.promise;
        },

        getStorage : function (storageId) {
            let request = {
                method: "GET",
                url: "http://192.168.1.8:8080/api/v1/storage/" + storageId,
                headers: {
                    Authorization: "Bearer " + localStorage["token"]
                }
            }

            return $http(request);
        }
    }
})
