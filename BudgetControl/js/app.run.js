(function () {
	'use strict';
	angular
        .module('budgetApp')
        .run(run);

	run.$inject = ['$rootScope', '$state', 'ngProgressFactory', 'authInfo', '$log', '$timeout', 'bcBuilder', 'msgService']
	function run($rootScope, $state, ngProgressFactory, authInfo, $log, $timeout, bcBuilder, msgService) {
		//Initial Progressbar
        var color = ['#DD2C00', '#FBC02D', '#F06292', '#4CAF50', '#FF9800', '#18FFFF', '#EA80FC'];
        var day = new Date().getDay();
		$rootScope.progressbar = ngProgressFactory.createInstance();
		$rootScope.progressbar.setColor(color[day || 2]); //FBC02D  //FF8A65
		$rootScope.progressbar.setHeight('6px');

        $rootScope.isProgressbarRun = false;

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
            $rootScope.isProgressbarRun = true;
		});

        $rootScope.$on('loading:finish', function () {
            if ($rootScope.isProgressbarRun) {
                $timeout(function () {
                    $rootScope.progressbar.complete();
                    $rootScope.isProgressbarRun = false;
                }, 1000)
            }
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