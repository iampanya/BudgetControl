'use strict';

budgetApp.factory('apiIdmService', ['$resource', function ($resource) {
    var idmApi = {}

    idmApi.employee = function () {
        return $resource('idm/getemployeeprofile?empno=:empno');
    }

    //idmApi.employee = function () {
    //    return $resource('data/employee/:id')
    //}

    //idmApi.costcenter = function () {
    //    return $resource('data/costcenter/:id');
    //}

    //idmApi.budget = function () {
    //    return $resource('data/budget/:id', null, {
    //        'save': { method: 'POST' },
    //        'update': { method: 'PUT' },
    //        'remove': { method: 'DELETE' }
    //    });
    //}


    return idmApi
}])