
;
/************** LoginCtrl ************/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('LoginCtrl', LoginCtrl);

    LoginCtrl.$inject = ['$state', '$log', 'userApi', 'authInfo', 'msgService']
    function LoginCtrl($state, $log, userApi, authInfo, msgService) {
        var vm = this;
        vm.login = login; //function submit form login
        vm.account = {
            username: '',
            password: ''
        };

        resetBtnSubmit();

        function login() {
            disableBtnSubmit();
            userApi.login()
                .send(vm.account)
                .$promise.then(callApiSuccess, callApiFailed);
        }


        function callApiSuccess(data) {
            if (data.isSuccess) {
                authInfo.setSession(data.Result);
                $state.go('home');
            }
            else {
                callApiFailed(data.Message);
            }
            resetBtnSubmit();
        }

        function callApiFailed(e) {
            msgService.setBottomErrorMsg(e);
            $log.error(e);
            resetBtnSubmit();
        }

        function resetBtnSubmit() {
            vm.btnSubmit = {
                name: 'ตกลง',
                disbled: false
            }
        }
        function disableBtnSubmit() {
            vm.btnSubmit = {
                name: 'กำลังเข้าสู่ระบบ . . .',
                disbled: true
            }
        }
    }

})();

/************** AuthCtrl ************/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('AuthCtrl', AuthCtrl);

    AuthCtrl.$inject = ['$scope', '$state', '$log', 'userApi', 'authInfo']
    function AuthCtrl($scope, $state, $log, userApi, authInfo) {
        var vm = this;
        vm.logout = logout;

        function logout() {
            userApi.logout().send().$promise.then(callApiSuccess, callApiFailed);
        }

        $scope.$watch(authInfo.isLoggedIn, function (value, oldValue) {
            if (!value && oldValue) {
                console.log('disconnect')
                loggedout()
            }

            if (value) {
                console.log("Connect")
                console.log(value);
                loggedin()
            }

        }, true);
        
        function loggedin() {
            vm.userinfo = authInfo.getUser();
            vm.isAuthen = true;
        }

        function loggedout() {
            vm.userinfo = {};
            vm.isAuthen = false;
            $state.go('login');
        }

        function callApiSuccess(data) {
            authInfo.clearSession();
        }

        function callApiFailed(e) {
            $log.error(error);
        }
    }

})();

/************** ProfileCtrl ************/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('ProfileCtrl', ProfileController)

    ProfileController.$inject = ['$scope']
    function ProfileController($scope) {

    }

})();

/************** PasswordCtrl ************/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('PasswordCtrl', PasswordController)

    PasswordController.$inject = ['$log', 'userApi', 'authInfo']

    function PasswordController($log, userApi, authInfo) {
        var vm = this;
        vm.form = {
            UserName: '',
            Password: '',
            NewPassword: '',
        };
        vm.submit = changePassword;
        vm.isLoading = false;
        vm.msg = {
            haveMessage: false,
            mclass: '',
            mheader: '',
            mtext: ''
        };
        vm.hideMessage = msgHide;

        // 1. Get user data from authInfo service
        var user = authInfo.getUser();

        // 2. If not found then get from userApi
        // 3. Initial form data
        if (!user) {
            userApi.getAuthen().get().$promise.then(getAuthenSuccess, getAuthenFailed);
        } else {
            initForm(user);
        }

        // Get Authen
        function getAuthenSuccess(data) {
            if (data.isSuccess) {
                initForm(data.Result.User)
            }
            else {
                $log.log(data)
            }
        }

        function getAuthenFailed(e) {
            $log.error(e);
        }

        function initForm(pUser) {
            vm.form.UserName = pUser.UserName;
        }

        // Change Password Function
        function changePassword() {
            vm.isLoading = true
            userApi.changePass().send(vm.form).$promise.then(changePasswordSuccess, changePasswordFailed);
        }


        function changePasswordSuccess(data) {
            $log.info(data)
            if (data.isSuccess) {
                msgSuccess('เปลี่ยนรหัสผ่านสำเร็จ');
            }
            else {
                changePasswordFailed(data.Message);
            }
            vm.isLoading = false;
        }

        function changePasswordFailed(e) {
            $log.error(e);
            msgFailed(e);
            vm.isLoading = false;
        }



        //Message function
        function msgBuilder(haveMessage, mclass, mheader, mtext) {
            vm.msg.haveMessage = haveMessage;
            vm.msg.mclass = mclass;
            vm.msg.mheader = mheader;
            vm.msg.mtext = mtext;
        }

        function msgSuccess(mtext) {
            msgBuilder(true, 'alert-success', '', mtext);
        }

        function msgFailed(mtext) {
            msgBuilder(true, 'alert-danger', '', mtext);
        }
        function msgHide() {
            msgBuilder(false);
        }
    }

})();





