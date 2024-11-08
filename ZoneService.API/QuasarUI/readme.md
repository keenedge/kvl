# Alpha Control Web UI

## Restore Dev Environment

    install nodejs 8.9

    clone this repo
    cd to root folder

    npm install -g quasar-cli@0.15.20

    npm install
    npm install --savedev

## run dev

    quasar dev


## release

    quasar build

build files are generated to ../wwwroot folder.  So dotnet poublish will use them.

Instal lPublish folder with sc ZoneApi.exe


    xcopy dist\spa-mat\* c:\ALPHAWEB\DEV\WebUI /s /e