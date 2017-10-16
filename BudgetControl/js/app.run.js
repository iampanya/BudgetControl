﻿(function () {
	'use strict';
	angular
        .module('budgetApp')
        .run(run);

	run.$inject = ['$rootScope', '$state', 'ngProgressFactory', 'authInfo', '$log', 'bcBuilder', 'msgService']
	function run($rootScope, $state, ngProgressFactory, authInfo, $log, bcBuilder, msgService) {
		//Initial Progressbar
		$rootScope.progressbar = ngProgressFactory.createInstance();
		$rootScope.progressbar.setColor('#FBC02D'); //FBC02D  //FF8A65
		$rootScope.progressbar.setHeight('3px');

		$rootScope.$on('$stateChangeStart', function (event, toState) {
		    if (toState.name !== 'login' && !authInfo.isLoggedIn()) {
		        event.preventDefault();
		        $state.go('login');
		    }
		})

		$rootScope.$on('$stateChangeSuccess', function (event, toState) {
		    bcBuilder.setbc(toState.name);
		    msgService.clearMsg();

		})


		$rootScope.$on('loading:progress', function () {
			$rootScope.progressbar.start();
		});

		$rootScope.$on('loading:finish', function () {
			$rootScope.progressbar.complete();
		});
	}
})();



(function () {
	'use strict';
	angular
        .module('budgetApp')
        .run(login)

	login.$inject = ['$window', '$log', 'userApi', 'authInfo']
	function login($window, $log, userApi, authInfo) {

	    if ($window.sessionStorage["userInfo"] !== 'null' && $window.sessionStorage["userInfo"] !== undefined) {
	        var userInfo = JSON.parse($window.sessionStorage["userInfo"]);
	        authInfo.setSession(userInfo);
	    }

	    else {

		    userApi.getAuthen().get().$promise.then(function (data) {
			    if (data.isSuccess) {
			        authInfo.setSession(data.Result);
			    }
		    }, function (error) {
		        $log.error(error);
		    })

	    }
	}


})();

//for test login
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .run(run)
        .config(config)
        .controller('TestCtrl', TestCtrl);

    function run($http) {
        $http.defaults.headers.common.Authorization = 'Basic YmVlcDpib29w';
    }

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('test', {
                url: '/test',
                templateUrl: 'Test/index',
                controller: 'TestCtrl',
                controllerAs: 'vm'
            });
    }

    function TestCtrl($http) {
        var vm = this;
        vm.test = 'test control';
        $http.get('test/budgets').then(successCallback, errorCallback);

        function successCallback(data) {
            console.log(data);
        }

        function errorCallback(e) {
            console.log(e);
        }
    }

})();