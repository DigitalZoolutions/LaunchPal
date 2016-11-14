app.controller('LaunchCtrl', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {

    $scope.Title = "LaunchPal - The go-to rocket launch tracker!";
    $scope.Launch;
    $scope.LoadMessage
    var counter;
    var mytimeout = null; // the current timeoutID

    window.onblur = function () {
        window.onfocus = function () {
            counter = 0;
            $scope.stopTimeout();
        }
    }

    // Get the latest launch from launchlibrary
    var getLaunch = function () {
        return $http.get('http://localhost:57334/api/LaunchLibrary/')
        //return $http.get('http://launchpal.digitalzoolutions.com/api/LaunchLibrary/')
            .then(
            function (payload) {
                return payload.data;
            });
    }
    
    $scope.onTimeout = function () {
        if (counter === 0) {
            $scope.stopTimeout();
            return;
        }
        counter--;
        $scope.day = Math.floor(counter / 60 / 60 / 24) % 365;
        $scope.hour = Math.floor(((counter / 60 / 60)) % 24);
        $scope.minute = Math.floor(((counter / 60)) % 60);
        $scope.second = Math.floor((counter % 60));
        mytimeout = $timeout($scope.onTimeout, 1000);
    };

    // actual timer method, counts down every second, stops on zero
    $scope.startTimer = function () {
        getLaunch().then(function (launch) {
            $scope.LoadMessage = "is planed to be launched in";
            $scope.Launch = launch;
            var t1 = new Date(launch.net);
            var t2 = new Date();
            var dif = t1.getTime() - t2.getTime();
            counter = dif / 1000;
            toastSuccess();
        }, function () {
            $scope.LoadMessage = "No launch was loaded";
            toastFailed();
        });
        mytimeout = new $timeout($scope.onTimeout, 1000);
    };
    
    // triggered, when the timer stops, you can do something here, maybe show a visual indicator or vibrate the device
    $scope.stopTimeout = function () {
        $timeout.cancel(mytimeout);
        console.log("Timer Stopped");
        //$scope.startTimer();
    }

    // Trigger if the scope is destroyed
    $scope.$on("$destroy", function (event) {
        console.log("I am leaving");
        $timeout.cancel(mytimeout);
    });

    var toastSuccess = function () {
        Command: toastr["info"]("The next launch was successfully loaded from Launchlibrary.net", "Launch fetched")

        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "500",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    }

    var toastFailed = function () {
        Command: toastr["error"]("The next launch was unable to load from Launchlibrary.net", "Fetch failed")

        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "500",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    }

}]);