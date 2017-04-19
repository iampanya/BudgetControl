﻿;
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
                templateUrl: 'Budgets/Edit',
                controller: 'EditBudgetCtrl',
                controllerAs: 'vm'
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
            
            // Payment page
            .state('payment', {
                url: '/payment',
                templateUrl: 'Payments/Index',
                controller: 'PaymentCtrl',
                controllerAs: 'vm'
            })
            .state('newpayment', {
                url: '/payment/create',
                templateUrl: 'Payments/Create',
                controller: 'CreatePaymentCtrl',
                controllerAs: 'vm'
            })

            .state('editpayment', {
                url: '/payment/edit/:id',
                templateUrl: 'Payments/Create',
                controller: 'EditPaymentCtrl',
                controllerAs: 'vm'
            })

            .state('viewpayment', {
                url: '/payment/:id',
                templateUrl: 'Payments/Details',
                controller: 'DetailPaymentCtrl',
                controllerAs: 'vm'
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

            // Report
            .state('summaryreport', {
                url: '/report/summary',
                templateUrl: 'Report/Summary',
                controller: 'ReportCtrl',
                controllerAs: 'vm'
            })
            
            .state('individualreport', {
                url: '/report/individual/:id',
                templateUrl: 'Report/Individual',
                controller: 'IndividualReportCtrl',
                controllerAs: 'vm'
            })


            // Error
            .state('error401', {
                url: '/error401',
                templateUrl: 'Home/Error401'
            })
            
    }

})();