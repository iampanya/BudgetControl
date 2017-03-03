//budgetApp.config(function ($routeProvider) {
//    $routeProvider
//        .when('/', {
//            templateUrl: 'Home/Home'
//        })

//        /************************************/
//        .when('/budget', {
//            templateUrl: 'Budgets/Index',
//            controller: 'BudgetController'
//        })
//        .when('/budget/create', {
//            templateUrl: 'Budgets/Create',
//            controller: 'CreateBudgetController'
//        })
//        .when('/budget/edit/:id', {
//            templateUrl: 'Budgets/Create',
//            controller: 'EditBudgetController'
//        })
//        .when('/budget/upload', {
//            templateUrl: 'Budgets/Upload',
//            controller: 'UploadBudgetController'
//        })
//        .when('/budget/:id', {
//            templateUrl: 'Budgets/Details',
//            controller: 'DetailBudgetController'
//        })

//        /***********************************/
//        .when('/payment', {
//            templateUrl: 'Payments/Index',
//            controller: 'PaymentController'
//        })
//        .when('/payment/create', {
//            templateUrl: 'Payments/Create',
//            controller: 'CreatePaymentController'
//        })
//        .when('/payment/edit/:id', {
//            templateUrl: 'Payments/Create',
//            controller: 'EditPaymentController'
//        })
//        .when('/payment/:id', {
//            templateUrl: 'Payments/Details',
//            controller: 'DetailPaymentController'
//        })

//    /***********************************/
//        .when('/user', {
//            templateUrl: 'User/Index',
//            controller: ''
//        })
//        .when('/signin', {
//            templateUrl: 'User/Signin',
//            controller: 'SigninController'
//        })
//})



(function () {
    angular
        .module('budgetApp')
        .config(config)

    config.$inject = ['$stateProvider', '$urlRouterProvider']
    function config($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/home')

        $stateProvider
            .state('home', {
                url: '/home',
                templateUrl: 'Home/Home'
            })

            .state('budget', {
                url: '/budget',
                templateUrl: 'Budgets/Index',
                controller: 'BudgetCtrl',
                controllerAs: 'vm'
            })
            .state('newbudget', {
                url: '/budget/create',
                templateUrl: 'Budgets/Create',
                controller: 'CreateBudgetController'
            })
            .state('editbudget', {
                url: '/budget/edit/:id',
                templateUrl: 'Budgets/Create',
                controller: 'EditBudgetController'
            })
            .state('uploadbudget', {
                url: '/budget/upload',
                templateUrl: 'Budgets/Upload',
                controller: 'UploadCtrl',
                controllerAs: 'vm'
            })
            .state('viewbudget', {
                url: '/budget/:id',
                templateUrl: 'Budgets/Details',
                controller: 'DetailBudgetCtrl',
                controllerAs: 'vm'
            })
            
            //
            .state('payment', {
                url: '/payment',
                templateUrl: 'Payments/Index',
                controller: 'PaymentController'
            })
            .state('newpayment', {
                url: '/payment/create',
                templateUrl: 'Payments/Create',
                controller: 'CreatePaymentController'
            })
            .state('editpayment', {
                url: '/payment/edit/:id',
                templateUrl: 'Payments/Create',
                controller: 'EditPaymentController'
            })
            .state('viewpayment', {
                url: '/payment/:id',
                templateUrl: 'Payments/Details',
                controller: 'DetailPaymentController'
            })

            .state('user', {
                url: '/user',
                templateUrl: 'User/Index'
            })

            .state('changepassword', {
                url: '/changepassword',
                templateUrl: 'User/ChangePassword',
                controller: 'PasswordCtrl',
                controllerAs: 'vm'
            })

            .state('login', {
                url: '/login',
                templateUrl: 'User/Signin',
                controller: 'LoginCtrl',
                controllerAs: 'vm'
            })
            
    }

})();