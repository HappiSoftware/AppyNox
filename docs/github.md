# GitHub Configurations

<br>

#### <a id="add-pfx-secrets-to-github-for-workflow" ></a> 1. Add pfx Secrets to Github for Workflow

<details>
   <summary><b>Click to Expand</b></summary>

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

<br>
<br>

#### <a id="add-appsetting-secrets-to-github-for-workflow" ></a> 2. Add appsetting Secrets to Github for Workflow

<details>
   <summary><b>Click to Expand</b></summary>

1.  **Convert appsetting Files to Single Line Json**

    Use an online website to convert the appsetting files to single line json. Such as https://www.text-utils.com/json-formatter/

    **Important!** Don't forget to remove the `comments` before converting the jsons.

    <br>

2.  **Create Secrets In Repository**

    Navigate to repository > `Secrets and variables` > Actions and create the necessary secrets.

    Example: secret name `COUPON_APPSETTINGS_STAGING`, paste the single line json to content.

<br>

3. **Modify workflow**

   Add the following content to workflow.

```yml
- name: Create AppSettings for Microservice A
  run: |
    echo '${{ secrets.APPSETTINGS_MICROSERVICE_A_STAGING }}' | base64 --decode > path/to/MicroserviceA/appsettings.Staging.json

- name: Create AppSettings for Microservice B
  run: |
    echo '${{ secrets.APPSETTINGS_MICROSERVICE_B_STAGING }}' | base64 --decode > path/to/MicroserviceB/appsettings.Staging.json
```

</details>
