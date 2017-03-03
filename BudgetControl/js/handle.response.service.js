; (function () {
    'use strict';
    angular
        .module('budgetApp')
        .factory('handleResponse', handleResponse);

    handleResponse.$inject = ['msgService'];

    function handleResponse(msgService) {
        var fn = {
            respondSuccess: respondSuccess,
            respondError: respondError
        };

        return fn;

        function respondSuccess(response) {
            if (response.isSuccess) {
                return response.Result;
            }
            else {
                respondError(response.Message);
            }
        }

        function respondError(response, keepto) {
            msgService.setErrorMsg(response);
        }
    }

})();