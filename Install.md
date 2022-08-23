# lazystack.io install
We are using CSS Foundation.

https://get.foundation/sites/docs/installation.html

Install Steps:

1. Install nvm-windows
   1. https://github.com/coreybutler/nvm-windows#installation--upgrades
2. git clone https://github.com/InSciCo/inscico.github.io.git
3. mv insico.github.io.git LazyStack.io 
4. cd LazyStack.io
5. To get python 2.7
   1. npm install --global windows-build-tools
   2. npm config set python "%USERPROFILE%\.windows-build-tools\python27\python.exe"
6. npm update 
7. then run with
   1. npm start


Note: This latest version of Foundation does not require we use an older version of node. Verified that node 16.16.0 works for instance.










