//'use strict';

//budgetApp.factory('userFactory', ['$resource', function ($resource) {
//    var user = {}
//    user.signin = function () {
//        return $resource('user/login',
//            {},
//            { send: { method: 'POST' } })
//    }

//    user.logout = function () {
//        return $resource('user/logout',
//            {},
//            { send: { method: 'POST' } })
//    }

//    user.Authen = function () {
//        return $resource('user/getauthen')
//    }

//    user.current = function () {
//        return $resource('user/current')
//    }

//    return user
//}])

; (function () {
    'use strict';
    angular
        .module('budgetApp')
        .factory('userApi', userApi)
        .factory('authInfo', authInfo)

    userApi.$inject = [ '$resource'];

    function userApi($resource) {
        var api = {
            login: login,
            logout: logout,
            getAuthen: getAuthen,
            changePass: changePass,
            current: current
        }
        return api

        function login () {
            return $resource('user/login',
                {},
                { send: { method: 'POST' } })
        }

        function logout () {
            return $resource('user/logout',
                {},
                { send: { method: 'POST' } })
        }

        function getAuthen() {
            return $resource('user/getauthen')
        }

        function changePass() {
            return $resource('user/changepassword',
                {},
                { send: { method: 'POST' } })
        }

        function current() {
            return $resource('user/current')
        }
    }

    authInfo.$inject = ['$window'];
    function authInfo($window) {
        var session
        var user
        var auth = {
            setSession: setSession,
            getSession: getSession,
            clearSession: clearSession,
            getUser: getUser,
            isLoggedIn: isLoggedIn
        }
        return auth;

        function setSession(aSession) {
            session = aSession
            user = session.User
            $window.sessionStorage["userInfo"] = JSON.stringify(session);
        }

        function getSession() {
            return session;
        }

        function clearSession() {
            $window.sessionStorage["userInfo"] = null;
            session = undefined;
            user = undefined;
        }

        function getUser() {
            return user;
        }



        function isLoggedIn() {
            return (session) ? session : false
        }
    }

})();