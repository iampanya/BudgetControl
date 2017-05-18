/****************** Footer Controller ************************/
(function () {
    angular.module('budgetApp')
        .controller('FooterCtrl', FooterCtrl);

    FooterCtrl.$inject = ['$uibModal']
    function FooterCtrl($uibModal) {
        var vm = this;
        vm.aboutus = aboutus;

        function aboutus() {
            console.log('aboutus');
            openModal('lg');
        }

        function openModal(size, parentSelector) {
            var parentElem = parentSelector ?
                angular.element($document[0].querySelector('.modal-demo ' + parentSelector)) : undefined;
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'Home/AboutUs',
                controller: 'AboutUsCtrl',
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

    function AboutUsCtrl() {
        var vm = this;

    }

})();
