# Alpha Control Web UI

## Restore Dev Environment

    install nodejs 8.9

    clone this repo
    cd to root folder

    npm install -g quasar-cli@1.0.0

    npm install
    npm install --savedev

uses quasar 1.0.0 defined in packackage.json


## run dev

    quasar dev


## release

    quasar build

build files are generated to dist folde.  Copy these file to [IIS]\[Server]\[WebApp].  For example.

    xcopy dist\spa-mat\* c:\ALPHAWEB\DEV\WebUI /s /e




