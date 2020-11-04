import { Button } from 'antd';
import React from 'react';

const ImageUploader = ({ onFileChange, onFileUpload, fileSelected, ...props }) => {
    const container =
        <>
            <label
                htmlFor="inputFile" className="ant-btn ant-btn-primary"
                style={{ minWidth: 150, margin: 10 }}>
                <input
                    id="inputFile"
                    onChange={onFileChange}
                    type="file"
                    style={{ display: "none" }} />
                    Select file
            </label>

            <Button
                style={{ minWidth: 150, margin: 10 }}
                type="primary"
                size="medium"
                disabled={!fileSelected}
                onClick={onFileUpload}>Upload</Button>
        </>
    return container;
};

export default ImageUploader;