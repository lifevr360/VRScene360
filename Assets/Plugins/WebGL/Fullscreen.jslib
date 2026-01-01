mergeInto(LibraryManager.library, {

  EnterFullscreen: function () {
    var canvas = document.getElementById("unity-canvas");
    if (!canvas) return;

    if (!document.fullscreenElement) {
      if (canvas.requestFullscreen) {
        canvas.requestFullscreen();
      } else if (canvas.webkitRequestFullscreen) {
        canvas.webkitRequestFullscreen();
      }
    }
  },

  ExitFullscreen: function () {
    if (document.fullscreenElement) {
      if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.webkitExitFullscreen) {
        document.webkitExitFullscreen();
      }
    }
  },

  IsFullscreen: function () {
    return !!document.fullscreenElement;
  }

});
