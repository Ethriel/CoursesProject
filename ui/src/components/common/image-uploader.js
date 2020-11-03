import { Button } from 'antd';
import React from 'react';

const ImageUploader = ({ onFileChange, onFileUpload, ...props }) => {
    const container =
        <>
            <label
                htmlFor="inputFile" className="ant-btn ant-btn-primary"
                style={{ minWidth: 150, marginBottom: 10 }}>
                <input
                    id="inputFile"
                    onChange={onFileChange}
                    type="file"
                    style={{ display: "none" }} />
                    Select file
            </label>

            <Button
                style={{ minWidth: 150, marginBottom:10 }}
                type="primary"
                size="medium"
                onClick={onFileUpload}>Upload</Button>
        </>
    return container;
};

export default ImageUploader;