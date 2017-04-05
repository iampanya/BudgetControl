; (function () {
    'use strict';
    angular
        .module('budgetApp')
        .factory('handleResponse', handleResponse);

    handleResponse.$inject = ['msgService', '$state'];

    function handleResponse(msgService, $state) {
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
                return false;
            }
        }

        function respondError(response, keepto) {
            if (response.status === 401) {
                $state.go('error401');
            }
            else {
                msgService.setErrorMsg(response);
            }
        }
    }

})();