'use strict';

var budgetApp = angular.module('budgetApp', ['ngRoute', 'ngResource', 'ngAnimate', 'ngProgress', 'ui.router', 'ui.bootstrap', 'ui.grid', 'ui.grid.resizeColumns']);


(function () {
    angular.module('budgetApp').factory('httpInterceptor', ['$q', '$rootScope',
        function ($q, $rootScope) {
            var loadingCount = 0;
        return {
            request: function (config) {
                if (++loadingCount === 1) {
                    $rootScope.$broadcast('loading:progress');
                }
                return config || $q.when(config);
            },

            response: function (response) {
                if (--loadingCount === 0) {
                    $rootScope.$broadcast('loading:finish');
                }
                return response || $q.when(response);
            },

            responseError: function (response) {
                if (--loadingCount === 0) {
                    $rootScope.$broadcast('loading:finish');
                }
                if (response.status === 401) {
                    console.log('unauthorize');
                    console.log(response);
                }
                return $q.reject(response);
            }
        };
    }

    ]).config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('httpInterceptor');
    }]);

})();


