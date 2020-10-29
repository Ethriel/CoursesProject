import React from 'react';
import { Card } from 'antd';
const { Meta } = Card;

const MapCards = (elements, onClickHandler) => {
    const cards = elements.map((elem) => {
        let src = elem.cover.includes('http') ? elem.cover : `https://localhost:44382/${elem.cover}`;
        const cover =
            <div className="img-container-my">
                <img className="img-card-my"
                    style={{ margin: "0 auto" }}
                    alt="No"
                    src={src} />
            </div>
        return <Card
            className="ant-card-my"
            hoverable
            key={elem.id}
            cardid={elem.id}
            title={elem.title}
            type={"inner"}
            size={"small"}
            onClick={onClickHandler}
            cover={cover}>
            <Meta description={cutDescr(elem.description)} />
        </Card>
    });
    return cards;
}

const cutDescr = (descr) => {
    let index = descr.indexOf(".", 0);
    return descr.substr(0, index);
}

export default MapCards;