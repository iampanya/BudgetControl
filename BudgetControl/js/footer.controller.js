/****************** Footer Controller ************************/
(function () {
    angular.module('budgetApp')
        .controller('FooterCtrl', FooterCtrl);

    FooterCtrl.$inject = ['$uibModal']
    function FooterCtrl($uibModal) {
        var vm = this;
        vm.aboutus = aboutus;
        vm.notice = notice;
        vm.document = document;
        vm.contactus = contactus;

        function aboutus() {
            console.log('aboutus');
            openModal('lg', 'Home/AboutUs', 'AboutUsCtrl');
        }

        function notice() {
            openModal('lg', 'Home/Notice', 'NoticeCtrl');
        }

        function document() {
            openModal('lg', 'Home/Document', 'DocumentCtrl');
        }

        function contactus() {
            openModal('lg', 'Home/ContactUs', 'ContactUsCtrl');
        }
        

        function openModal(size, templateUrl, controller, parentSelector) {
            var parentElem = parentSelector ?
                angular.element($document[0].querySelector('.modal-demo ' + parentSelector)) : undefined;
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: templateUrl,
                controller: controller,
                controllerAs: 'vm',
                size: size,
                appendTo: parentElem
            });

            modalInstance.result.then(function (payment) {
                if (payment) {
                    var index = vm.payments.findIndex(function (obj) {
                        return obj.PaymentID == payment.PaymentID;
                    });
                    vm.payments.splice(index, 1);
                }
            });
        };
    }

})();


/****************** AboutUs Controller ***********************/
(function () {
    angular.module('budgetApp')
        .controller('AboutUsCtrl', AboutUsCtrl);

    AboutUsCtrl.$inject = ['$uibModalInstance'];
    function AboutUsCtrl($uibModalInstance) {
        var vm = this;
        vm.close = close;

        function close() {
            $uibModalInstance.close();
        }

    }

})();


/****************** Notice Controller ***********************/
(function () {
    angular.module('budgetApp')
        .controller('NoticeCtrl', NoticeCtrl);

    NoticeCtrl.$inject = ['$uibModalInstance']
    function NoticeCtrl($uibModalInstance) {
        var vm = this;
        vm.close = close;

        function close() {
            $uibModalInstance.close();
        }

    }

})();

/****************** Document Controller ***********************/
(function () {
    angular.module('budgetApp')
        .controller('DocumentCtrl', DocumentCtrl);

    DocumentCtrl.$inject = ['$uibModalInstance']
    function DocumentCtrl($uibModalInstance) {
        var vm = this;
        vm.close = close;

        function close() {
            $uibModalInstance.close();
        }

    }

})();

/****************** ContactUs Controller ***********************/
(function () {
    angular.module('budgetApp')
        .controller('ContactUsCtrl', ContactUsCtrl);

    ContactUsCtrl.$inject = ['$uibModalInstance']
    function ContactUsCtrl($uibModalInstance) {
        var vm = this;
        vm.close = close;

        function close() {
            $uibModalInstance.close();
        }

    }

})();