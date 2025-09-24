use std::ffi::{CStr, CString, c_int};
use std::os::raw::c_char;

use gxt::GxtError;
use serde::Deserialize;

#[derive(Deserialize)]
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

fn process(req: Request) -> Result<String, GxtError> {
    match req {
        Request::MakeKey {} => Ok(gxt::make_key()),
        Request::MakeIdCard { key, meta } => gxt::make_id_card(&key, meta),
        Request::Verify { msg } => gxt::verify_message::<serde_json::Value>(&msg)
            .and_then(|e| serde_json::to_string(&e).map_err(From::from)),
        Request::Encrypt {
            key,
            to,
            payload,
            parent,
        } => gxt::encrypt_message(&key, &to, &payload, parent),
        Request::Decrypt { key, msg } => gxt::decrypt_message::<serde_json::Value>(&msg, &key)
            .and_then(|e| serde_json::to_string(&e).map_err(From::from)),
    }
}

/// .
///
/// # Safety
///
/// .
#[unsafe(no_mangle)]
pub unsafe extern "C" fn gxt_execute(input: *const c_char, result: *mut c_int) -> *mut c_char {
    if input.is_null() {
        return std::ptr::null_mut();
    }

    let cstr = unsafe { CStr::from_ptr(input) };
    let s = match cstr.to_str() {
        Ok(s) => s,
        Err(_) => return std::ptr::null_mut(),
    };

    unsafe { *result = 0 };

    if let Ok(req) = serde_json::from_str::<Request>(s) {
        if let Ok(output) = process(req) {
            if let Ok(cstr_out) = CString::new(output) {
                return cstr_out.into_raw();
            } else {
                unsafe { *result = 3 };
            }
        } else {
            unsafe { *result = 2 };
        }
    } else {
        unsafe { *result = 1 };
    }
    std::ptr::null_mut()
}

/// .
///
/// # Safety
///
/// .
#[unsafe(no_mangle)]
pub unsafe extern "C" fn free_rust_cstr(ptr: *mut c_char) {
    if ptr.is_null() {
        return;
    }
    unsafe {
        let _ = CString::from_raw(ptr);
    }
}
