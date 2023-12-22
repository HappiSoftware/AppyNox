# SSL Certificate

After cloning the repository, add development SSL certificates to the services for running them in a Docker container. Run the following commands to generate and trust the SSL certificates:

1. **Download OpenSSL**

   Navigate to https://slproweb.com/products/Win32OpenSSL.html and download OpenSSL installer.

   `(Win64 OpenSSL v3.2.0 Light)` version may differ.

<br>

2. **Edit Environment Variables**

   Add `C:\Program Files\OpenSSL-Win64\bin` to System variables. Path may change based on your installation location.

<br>

3. **Create `cert.config`**

   In a folder create `cert.config`.

<details>
    <summary>The content of the cert.config Example</summary>

```config
[req]
default_bits = 2048
prompt = no
default_md = sha256
x509_extensions = v3_req
distinguished_name = dn

[dn]
C=TR
ST=Sakarya
L=Sakarya
O=HappiSoftware
OU=NeonNinjas
emailAddress=happisoftware@gmail.com
CN = localhost

[v3_req]
subjectAltName = @alt_names

[alt_names]
DNS.1 = localhost
DNS.2 = appynox-gateway-ocelotgateway
```

</details>

<br>

4. **Create pfx and crt files**

Run the following codes in the terminal.

```bash
openssl req -new -nodes -newkey rsa:2048 -keyout appynox.key -out appynox.csr -config cert.config
openssl x509 -signkey appynox.key -in appynox.csr -req -days 365 -out appynox.crt -extfile cert.config -extensions v3_req
openssl pkcs12 -export -out appynox.pfx -inkey appynox.key -in appynox.crt -password pass:happi2023
```

<br>

5. **Adding the files to APIs**

   In Ocelot Gateway project there will be `'ssl'` folder. Move the `.pfx` file under `'ssl'` folder.

<br>

6. **Trust The Certificate**

   Just double click the appynox.pfx and `next next next`

<br>
<br>

# Advanced

<details>
   <summary><b>Add pfx Secrets to Github for Workflow</b></summary>

1.  **Extract The .pfx to Base64**

```bash
$fileContentBytes = Get-Content '{pfx-full-path}\appynox.pfx' -Encoding Byte

[System.Convert]::ToBase64String($fileContentBytes) | Out-File '{pfx-full-path}\appynox.txt'
```

<br>

2. **Create Secrets In Repository**

Navigate to repository > `Secrets and variables` > Actions and create 2 repository secrets called `PFX_CERTIFICATE` and `PFX_PASSWORD`. `PFX_CERTIFICATE` will be Base64 encoded .pfx and `PFX_PASSWORD` will be the password used to create the ssl.

<br>

3. **Modify workflow**

Add the following content to workflow.

```yml
- name: Trust self-signed certificate
        run: |
          echo "${{ secrets.PFX_CERTIFICATE }}" | base64 --decode > appynox_staging.pfx
          # Extract the certificate from the .pfx file
          openssl pkcs12 -in appynox_staging.pfx -clcerts -nokeys -out appynox_staging.crt -password pass:${{ secrets.PFX_PASSWORD }}
          # Trust the certificate
          sudo cp appynox_staging.crt /usr/local/share/ca-certificates/
          sudo update-ca-certificates
```

</details>
