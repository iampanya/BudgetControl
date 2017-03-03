/************ MESSAGE SERVICE **************/

(function () {
    'use strict';
    angular
        .module('budgetApp')
        .factory('msgService', msgService);

    msgService.$inject = ['$rootScope']
    function msgService($rootScope) {
        var msg = {
            setMsg: setMsg,
            clearMsg: clearMsg,
            setErrorMsg: setErrorMsg,
            setSuccessMsg: setSuccessMsg,
            setBottomErrorMsg: setBottomErrorMsg,
            setBottomSuccessMsg: setBottomSuccessMsg
        }

        return msg;

        function setMsg(type, text, position) {
            registerClearMsg();
            $rootScope.msg = {
                type: type,
                position: position || 'top',
                text: text
            }

            
        }

        function clearMsg() {
            $rootScope.msg = {
                type: '',
                position: '',
                text: ''
            }
        }
        
        function setErrorMsg(text) {
            setMsg('alert-danger', text);
        }

        function setSuccessMsg(text) {
            setMsg('alert-success', text);
        }

        function setBottomErrorMsg(text) {
            setMsg('alert-danger', text, 'bottom');
        }

        function setBottomSuccessMsg(text) {
            setMsg('alert-success', text, 'bottom');
        }

        function registerClearMsg() {
            $rootScope.clearMsg = function () {
                $rootScope.msg = {
                    type: '',
                    position: '',
                    text: ''
                }
            };
        }
    }

})();



/*************  Breadcrumb Builder *************/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .factory('bcBuilder', bcBuilder);

    bcBuilder.$inject = ['$rootScope']
    function bcBuilder($rootScope) {
        var bc = {
            setbc: setbc,
            clearbc: clearbc
        };
        clearbc();

        return bc;


        function setbc(stateName) {
            clearbc();
            addHome();
            switch (stateName) {
                case 'home':
                    break;
                // Budget Route
                case 'budget':
                    addBudget();
                    break;
                case 'newbudget':
                    addBudget();
                    addItem('newbudget', 'เพิ่ม')
                    break;
                case 'editbudget':
                    addBudget();
                    addItem('editbudget', 'แก้ไข')
                    break;
                case 'uploadbudget':
                    addBudget();
                    addItem('uploadbudget', 'นำเข้าข้อมูลจาก SAP')
                    break;
                case 'viewbudget':
                    addBudget();
                    addItem('viewbudget', 'รายละเอียด')
                    break;

                // Payment Route
                case 'payment':
                    addPayment();
                    break;
                case 'newpayment':
                    addPayment();
                    addItem('newpayment', 'ตัดงบ')
                    break;
                case 'editpayment':
                    addPayment();
                    addItem('editpayment', 'แก้ไขการเบิก')
                    break;
                case 'viewpayment':
                    addPayment();
                    addItem('viewpayment', 'รายละเอียด')
                    break;
                
            }
            
            
            
        }

        function clearbc() {
            $rootScope.breadcrumb = [];
        }

        function getbc() {
            return $rootScope.breadcrumb;
        }

        function addHome() {
            addItem('home', 'หน้าแรก');
        }

        function addBudget() {
            addItem('budget', 'งบที่ได้รับ');
        }

        function addPayment() {
            addItem('payment', 'รายการเบิกจ่าย');
        }

        function addItem(state, text) {
            $rootScope.breadcrumb.push({
                state: state,
                text: text
            })
        }

     
    }

})();