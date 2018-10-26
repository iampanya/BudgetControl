

budgetApp.filter('unique', function () {
    // we will return a function which will take in a collection
    // and a keyname
    return function (collection, keyname) {
        // we define our output and keys array;
        var output = [],
            keys = [];

        // we utilize angular's foreach function
        // this takes in our original collection and an iterator function
        angular.forEach(collection, function (item) {
            // we check to see whether our object exists
            var key = item[keyname];
            // if it's not already part of our keys array
            if (keys.indexOf(key) === -1) {
                // add it to our keys array
                keys.push(key);
                // push this item to our final output array
                output.push(item);
            }
        });
        // return our array which should be devoid of
        // any duplicates
        return output;
    };
});


(function () {
    angular.module('budgetApp')
        .filter('total', total);

    function total(){
        return function (data, key) {
            if (angular.isUndefined(data) || angular.isUndefined(key))
                return 0;
            var sum = 0;
            angular.forEach(data, function (value) {
                //sum = sum + parseInt(value[key]);
                sum = sum + parseFloat(value[key]);
            });
            return sum;
        }
    }

})();

(function () {
    angular.module('budgetApp')
        .filter('negative', negative);

    function negative() {
        return function (input) {
            input = input ? input : 0;
            console.log(input);
            return input < 0 ? "+" + input : input;
        }
    }

})();

