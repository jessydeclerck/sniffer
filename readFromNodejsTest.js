const { spawn } = require("child_process");
const readline = require("readline");

let snifferProcess = spawn(
  "./bin/Release/netcoreapp3.0/win-x64/publish/sniffer.exe"
);

snifferProcess.stdout.pipe(process.stdout);

var rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

rl.on("line", function(line) {
  if (isNaN(parseInt(line))) {
    console.log("Please enter a number : ");
    return;
  }
  snifferProcess.stdin.write(parseInt(line) + "\n");
  rl.removeAllListeners();
  snifferProcess.stdout.unpipe(process.stdout);
});

