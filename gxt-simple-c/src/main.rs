use gxt::json;
use serde::Serialize;

#[derive(Serialize)]
#[serde(untagged)]
pub enum Request {
    MakeKey {},
    MakeIdCard {
        key: String,
        meta: serde_json::Value,
    },
    Verify {
        msg: String,
    },
    Encrypt {
        key: String,
        to: String,
        payload: serde_json::Value,
        parent: Option<String>,
    },
    Decrypt {
        key: String,
        msg: String,
    },
}

fn main() {
    let requests = vec![
        Request::MakeKey {},
        Request::MakeIdCard {
            key: "key".to_string(),
            meta: json!({"name":"bob"}),
        },
        Request::Encrypt {
            key: "key".to_string(),
            to: "id_card".to_string(),
            payload: json!({"hello": "world"}),
            parent: None,
        },
        Request::Decrypt {
            key: "key".to_string(),
            msg: "msg".to_string(),
        },
    ];
    println!("{}", serde_json::to_string_pretty(&requests).unwrap());
}
